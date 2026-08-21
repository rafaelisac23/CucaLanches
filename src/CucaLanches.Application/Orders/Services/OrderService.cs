using CucaLanches.Application.Addresses.Interfaces;
using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Orders.DTOs;
using CucaLanches.Application.Orders.Interfaces;
using CucaLanches.Application.Products.Interfaces;
using CucaLanches.Application.StoreSettings.Interfaces;
using CucaLanches.Application.Validators;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Orders.Services;

public class OrderService:IOrderService
{
    
    private readonly IOrderRepository _orderRepository;
    private readonly IStoreSettingRepository _storeSettingRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository,IStoreSettingRepository  
        storeSettingRepository,IAddressRepository addressRepository , IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _storeSettingRepository = storeSettingRepository;
        _addressRepository = addressRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderResponseDto> CreateAsync(CreateOrderRequestDto req)
    {
        var store = await _storeSettingRepository.Get();
        if (store is null || !store.IsOpen) throw new OrderRuleException("The Store is closed in this moment");

        var errors = OrderValidator.IsValid(req);
        
        if(errors.Any()) throw new ValidationException(errors);

        var address = await _addressRepository.GetByIdAsync(req.AddressId);
        
        if(address is null) throw new NotFoundException("This address is not found");

        if (address.ClientId != req.ClientId) throw new OrderRuleException("This Address don't belong to this client");

        if (!address.Neighborhood.IsAvaliable) throw new OrderRuleException("Delivery to this neighborhood is currently unavailable");

        var productIds = req.Items.Select(p => p.ProductId).Distinct().ToList();

        var products = await _productRepository.GetByIds(productIds);

        var order = new Order
        {
            ClientId = req.ClientId,
            AddressId = req.AddressId,
            PaymentMethod = req.PaymentMethod,
            CashChangeFor = req.CashChangeFor,
            DeliveryFee = address.Neighborhood.DeliveryFee
        };
        
        
        foreach (var item in req.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);

            if (product is null) throw new NotFoundException($"Product {item.ProductId} not found");
            
            if(!product.Active) throw new OrderRuleException("This product is not active");
            
            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                Description = item.Description
            });
        }
        
        order.RecalculateTotal();
        
        await _orderRepository.CreateAsync(order);

        var createdOrder = await _orderRepository.GetByIdAsync(order.Id);
        
        if(createdOrder is null) throw new NotFoundException("Order not found");

        var orderResponseDto = new OrderResponseDto
        {
            Id = createdOrder.Id,
            Items = createdOrder.Items.Select(i=> new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Description = i.Description,
                UnitPrice = i.UnitPrice
            }).ToList(),
            PaymentMethod = createdOrder.PaymentMethod,
            OrderNumber = createdOrder.OrderNumber,
            Status = createdOrder.Status,
            DeliveryFee = createdOrder.DeliveryFee,
            TotalPrice = order.TotalPrice,
            CreatedAt =  createdOrder.CreatedAt,
            AddressSummary = $"{address.StreetName}, {address.HouseNumber} - {address.Neighborhood.Name}",
            ClientName = createdOrder.Client.Name,
            ClientPhone = createdOrder.Client.Phone,
            CashChangeFor = createdOrder.CashChangeFor
        };
        
        return orderResponseDto;

    }
}