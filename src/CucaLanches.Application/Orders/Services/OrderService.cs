using CucaLanches.Application.Addresses.Interfaces;
using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Orders.DTOs;
using CucaLanches.Application.Orders.Interfaces;
using CucaLanches.Application.Products.Interfaces;
using CucaLanches.Application.StoreSettings.Interfaces;
using CucaLanches.Application.Validators;
using CucaLanches.Domain.Entities;
using CucaLanches.Domain.Enums;

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
            Status = createdOrder.Status.ToString(),
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

    public async Task<List<OrderResponseDto>> GetOrdersByDateAndOrderStatusAsync(DateTime date, OrderStatus status)
    {
        var validDate = date;
        var now = DateTime.Now;

        if (validDate > now)
        {
            throw new OrderRuleException("Invalid date: the date cannot be in the future.");
        }
        
        var orders = await _orderRepository.GetOrderByDateAndOrderStatus(date, status);

        var ordersResponse = orders.Select(o => new OrderResponseDto()
        {
            Id = o.Id,
            AddressSummary = $"{o.Address.StreetName}, {o.Address.HouseNumber} - {o.Address.Neighborhood.Name}",
            CashChangeFor = o.CashChangeFor,
            ClientName = o.Client.Name,
            OrderNumber = o.OrderNumber,
            ClientPhone = o.Client.Phone,
            CreatedAt = o.CreatedAt,
            Status = o.Status.ToString(),
            DeliveryFee = o.DeliveryFee,
            PaymentMethod = o.PaymentMethod,
            TotalPrice = o.TotalPrice,
            Items = o.Items.Select(i=> new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Description = i.Description,
                UnitPrice = i.UnitPrice
            }).ToList(),
        }).ToList();
        
        return ordersResponse;
    }

    public async Task<OrderResponseDto> GetOrderByIdAsync(int orderId)
    {
        if (orderId < 0)
        {
            throw new OrderRuleException("Invalid order id");
        }

        var order = await _orderRepository.GetByIdAsync(orderId);
        
        if(order is null) throw new NotFoundException("This order doesn't exist");

        var response = new OrderResponseDto()
        {
            Id = order.Id,
            AddressSummary = $"{order.Address.StreetName}, {order.Address.HouseNumber} - {order.Address.Neighborhood.Name}",
            CashChangeFor = order.CashChangeFor,
            ClientName = order.Client.Name,
            OrderNumber = order.OrderNumber,
            ClientPhone = order.Client.Phone,
            CreatedAt = order.CreatedAt,
            Status = order.Status.ToString(),
            DeliveryFee = order.DeliveryFee,
            PaymentMethod = order.PaymentMethod,
            TotalPrice = order.TotalPrice,
            Items = order.Items.Select(i=> new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Description = i.Description,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
        
        return response;

    }

    public async Task<OrderResponseDto> ChangeStatusAsync(int orderId, OrderStatus status)
    {
        if (orderId <= 0)
        {
            throw new NotFoundException("Order doesn't exist");
        }
        
        var order = await _orderRepository.GetByIdAsync(orderId);
        
        if (order is null) throw new NotFoundException("This order doesn't exist");
        
        order.AdvanceTo(status);

        await _orderRepository.ChangeStatusAsync(order);
        
        var alteredOrder = await _orderRepository.GetByIdAsync(orderId);
        
        if(alteredOrder is null) throw new NotFoundException("This order doesn't exist");
        
        var response = new OrderResponseDto()
        {
            Id = alteredOrder.Id,
            AddressSummary = $"{alteredOrder.Address.StreetName}, {alteredOrder.Address.HouseNumber} - {alteredOrder.Address.Neighborhood.Name}",
            CashChangeFor = alteredOrder.CashChangeFor,
            ClientName = alteredOrder.Client.Name,
            OrderNumber = alteredOrder.OrderNumber,
            ClientPhone = alteredOrder.Client.Phone,
            CreatedAt = alteredOrder.CreatedAt,
            Status = alteredOrder.Status.ToString(),
            DeliveryFee = alteredOrder.DeliveryFee,
            PaymentMethod = alteredOrder.PaymentMethod,
            TotalPrice = alteredOrder.TotalPrice,
            Items = alteredOrder.Items.Select(i=> new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Description = i.Description,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
        
        return response;
    }
    
    
}