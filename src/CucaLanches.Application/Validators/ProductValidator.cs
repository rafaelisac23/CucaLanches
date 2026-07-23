
using CucaLanches.Application.Products.DTOs;
using CucaLanches.Domain.Entities;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Validators;

public static class ProductValidator
{

    public static List<ValidationError> IsValid(ProductRequestDTO product)
    {
        var errors = new List<ValidationError>();

        if(product == null)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(product),
                Message = "Product is null"
            });

            return errors;
        }


        if(string.IsNullOrWhiteSpace(product.Name))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(product.Name),
                Message = "Product name is required"
            });
        }
        else if(product.Name.Length > 120)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(product.Name),
                Message = "Product name is too long (120)"
            });
        }


        if(product.Price == null || product.Price <= 0)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(product.Price),
                Message = "Price must be greater than zero"
            });
        }


        if(product.Type == null)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(product.Type),
                Message = "Type is required"
            });
        }
        else if(!Enum.IsDefined(typeof(ProductType), product.Type))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(product.Type),
                Message = "Type is invalid"
            });
        }


        return errors;
    }
}