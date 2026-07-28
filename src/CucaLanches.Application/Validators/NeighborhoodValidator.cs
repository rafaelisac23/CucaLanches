using System.ComponentModel.DataAnnotations;
using CucaLanches.Application.Neighborhoods.DTOs;

namespace CucaLanches.Application.Validators;

public static class NeighborhoodValidator
{
    public static List<ValidationError> IsValid(NeighborhoodRequestDTO neighborhood)
    {
        var errors = new List<ValidationError>();


        if(neighborhood == null)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(neighborhood),
                Message = "Neighborhood is null"
            });

            return errors;
        }
        
      

        if (string.IsNullOrWhiteSpace(neighborhood.Name))
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(neighborhood.Name),
                    Message = "Name is required"
                }
            );
        }

        if (neighborhood.Name.Length > 80)
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(neighborhood.Name),
                    Message = "Name maximum length is 80 characters"
                }
            );
        }

        if (neighborhood.DeliveryFee < 0)
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(neighborhood.Name),
                    Message = "Delivery Fee isn't negative"
                }
            );
        }
        
        
        return errors;
    }
    
    public static List<ValidationError> IsValidForUpdate(NeighborhoodUpdateRequestDTO neighborhood)
    {
        var errors = new List<ValidationError>();
        
        
        if (neighborhood.Name is not null && neighborhood.Name.Length > 80)
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(neighborhood.Name),
                    Message = "Name maximum length is 80 characters"
                }
            );
        }

        if (neighborhood.DeliveryFee.HasValue && neighborhood.DeliveryFee < 0)
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(neighborhood.DeliveryFee),
                    Message = "Delivery Fee cannot be negative"
                }
            );
        }
        
        
        return errors;
    }
}