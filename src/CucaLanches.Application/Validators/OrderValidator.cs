using CucaLanches.Application.Orders.DTOs;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Validators;

public static class OrderValidator
{
    public static List<ValidationError> IsValid(CreateOrderRequestDto request)
    {
        var errors = new List<ValidationError>();

        if (request.ClientId <= 0)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.ClientId),
                Message = $"{nameof(request.ClientId)} must be greater than zero."
            });
        }
        
        if (request.AddressId <= 0)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.AddressId),
                Message = $"{nameof(request.AddressId)} must be greater than zero."
            });
        }

        if (!Enum.IsDefined(typeof(PaymentMethod), request.PaymentMethod))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.PaymentMethod),
                Message = $"Invalid payment method."
            });
        }
        
        if (request.PaymentMethod == PaymentMethod.Cash &&
            !request.CashChangeFor.HasValue)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.CashChangeFor),
                Message = "CashChangeFor is required when payment method is Cash."
            });
        }
        
        if (request.PaymentMethod != PaymentMethod.Cash &&
            request.CashChangeFor.HasValue)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.CashChangeFor),
                Message = "CashChangeFor should only be informed for cash payments."
            });
        }

        if (!request.Items.Any())
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.Items),
                Message = "Order must have at least one item."
            });
        }

        if (request.Items.Exists(p => p.Quantity <= 0))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.Items),
                Message = "Items must have at least one quantity."
            });
        }
        
        return errors;
    }
    
    
}