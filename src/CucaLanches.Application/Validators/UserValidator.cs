using CucaLanches.Application.Common;
using CucaLanches.Application.Users.DTOs;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Application.Validators;

public static class UserValidator
{

    public static List<ValidationError> IsValid(UserRequestDTO user)
    {
        var errors = new List<ValidationError>();

        
        if (string.IsNullOrWhiteSpace(user.Name)) 
            errors.Add(new ValidationError()
            {
                Field = nameof(user.Name),
                Message = "Name is required."
            });

        if (string.IsNullOrWhiteSpace(user.Email)) 
            errors.Add(new ValidationError()
        {
            Field = nameof(user.Email),
            Message = "Email is required."
        });
        
        if(!EmailValidator.IsValid(user.Email)) 
            errors.Add(new ValidationError()
        {
            Field = nameof(user.Email),
            Message = "Email isn't valid."
        });

        if (string.IsNullOrWhiteSpace(user.Password))
            errors.Add(new ValidationError()
            {
                Field = nameof(user.Password),
                Message = "Password is required."
            });

        if (!Enum.IsDefined(typeof(UserRole), user.Role))
            errors.Add(new ValidationError()
            {
                Field = nameof(user.Role),
                Message = "User role is not valid"
            });
        
        

       

        return errors;
    }
    
}