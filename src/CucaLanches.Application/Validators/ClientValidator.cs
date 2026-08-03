using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using CucaLanches.Application.Clients.DTOs;
using CucaLanches.Application.Common;

namespace CucaLanches.Application.Validators;

public static class ClientValidator
{
    public static List<ValidationError> IsValid(ClientRequestDTO request,string phone)
    {

        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrEmpty(request.Name))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.Name),
                Message = "Name is required"
            });
        }

        if (string.IsNullOrWhiteSpace(request.Phone) || string.IsNullOrEmpty(request.Phone))
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(request.Phone),
                    Message = "Phone is required"
                }
            );
        }
        
        if (phone.Length != 13)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.Phone),
                Message = "This Number is invalid - ( DDD + Number )"
            });
        }

        if (!string.IsNullOrEmpty(request.Email) && !EmailValidator.IsValid(request.Email))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.Phone),
                Message = "This email is invalid"
            });
        }
        
        
        
        return errors;

    }
}