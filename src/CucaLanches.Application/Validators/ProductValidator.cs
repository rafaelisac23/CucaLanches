
using CucaLanches.Application.Products.DTOs;
using CucaLanches.Domain.Entities;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Validators;

public static class ProductValidator
{

    public static List<ValidationError> IsValid(ProductRequestDTO product)
    {

        var errors = new List<ValidationError>();
        
        
        if (product == null)
        {
            errors.Add(new ValidationError
            {
                Name = nameof(product),
                Message = "Product is null"
            });
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            errors.Add(new ValidationError
            {
                 Name = nameof(product.Name),
                 Message = "Product name is required"
            });
        }

        if (product.Name.Length > 120)
        {
            errors.Add(new ValidationError
                {
                    Name = nameof(product.Name),
                    Message = "Product name is too long (120)"
                }
            );
        }

        if (product.Price <= 0)
        {
            errors.Add(new ValidationError
                {
                    Name = nameof(product.Price),
                    Message = "Price is greater than zero"
                }
            );
        }

        if (!Enum.IsDefined(product.Type))
        {
            errors.Add(new ValidationError
            {
                Name = nameof(product.Type),
                Message = "Type is invalid"
            });
        }

        
        return errors;
        
    }
    
}