using CucaLanches.Application.Addresses.DTOs;
using CucaLanches.Application.Common;

namespace CucaLanches.Application.Validators;

public static class AddressValidator
{
    public static List<ValidationError> IsValid(AddressRequestDTO request)
    {

        var errors = new List<ValidationError>();

        
        if (!request.ClientId.HasValue || request.ClientId <= 0)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.ClientId),
                Message = "ClientId is required"
            });
        }

        if (!request.NeighborhoodId.HasValue || request.NeighborhoodId <= 0)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.NeighborhoodId),
                Message = "NeighborhoodId is required"
            }); 
        }

        if (request.Cep is null || string.IsNullOrWhiteSpace(request.Cep))
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(request.Cep),
                    Message = "Cep is required"
                }
            );
        }

        if (request.Cep is not null)
        {
            if (request.Cep.Length !=8)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.Cep),
                    Message = "Cep is a invalid Cep ( 0000-000 )"
                });
            }
        }

        if (request.StreetName is null || string.IsNullOrWhiteSpace(request.StreetName))
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.StreetName),
                Message = "Street Name is required."
            });
        }

        if (request.StreetName is not null)
        {
            if (request.StreetName.Length > 150)
            {
                errors.Add(new ValidationError
                {
                    Field = nameof(request.StreetName),
                    Message = "Street Name is soo long"
                });
            }
        }

        if (request.HouseNumber is null || request.HouseNumber <= 0)
        {
            errors.Add(new ValidationError
            {
                Field = nameof(request.HouseNumber),
                Message = "House Number is required"
            });
        }
        
        return errors;

    }
    
    public static List<ValidationError> IsValidForUpdate(AddressRequestPatchDTO request)
    {

        var errors = new List<ValidationError>();
        
            if (request.NeighborhoodId.HasValue && request.NeighborhoodId <= 0)
            {
                errors.Add(new ValidationError
                    {
                        Field = nameof(request.NeighborhoodId),
                        Message = "NeighborhoodId is required"
                    }
                );
            }
        
        
        if (request.Cep is not null && request.Cep!.Length !=8)
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(request.Cep),
                    Message = "Cep is a invalid CEP (0000-000)"
                }
            );
        }

        if (request.StreetName is not null && request.StreetName!.Length > 150)
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(request.StreetName),
                    Message = "The street name is so long"
                }
            );
        }
        
        if (request.HouseNumber.HasValue && request.HouseNumber <= 0)
        {
            errors.Add(new ValidationError
                {
                    Field = nameof(request.HouseNumber),
                    Message = "Is a invalid houseNumber"
                }
            );
        }
        
        
        
        
        return errors;

    }


}