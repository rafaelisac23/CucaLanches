using CucaLanches.Application.Clients.DTOs;

namespace CucaLanches.Application.Validators;

public static class PhoneNumberValidator
{
    public static List<ValidationError> IsValid(string phone,IdentifyClientRequestDTO request)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrEmpty(phone))
        {
            errors.Add(new ValidationError
            {
                Field = "Phone",
                Message = "Phone number is required"
            });
        }

        if (phone.Length != 13)
        {
            errors.Add(new ValidationError
            {
                Field = "Phone",
                Message = "Phone number is not Valid ( DDD + PhoneNumber )"
            });
        }

        if (string.IsNullOrEmpty(request.Phone))
        {
            errors.Add(new ValidationError
            {
                Field = "Phone",
                Message = "Phone number is required"
            });
        }
        
        return errors;
    }
    
    public static List<ValidationError> IsValidForUpdate(string phone)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(phone))
        {
            errors.Add(new ValidationError
            {
                Field = "Phone",
                Message = "Phone number is required"
            });
        }

        if (phone.Length != 13)
        {
            errors.Add(new ValidationError
            {
                Field = "Phone",
                Message = "Phone number is not Valid ( DDD + PhoneNumber )"
            });
        }
        
        return errors;
    }
    
    
}