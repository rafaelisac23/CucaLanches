using System.Net;
using CucaLanches.Application.Addresses.DTOs;
using CucaLanches.Application.Clients.DTOs;
using CucaLanches.Application.Clients.Interfaces;
using CucaLanches.Application.Common;
using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Validators;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Clients.Services;

public class ClientService:IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository  clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public async Task<ClientResponseDTO> IdentifyCLient(IdentifyClientRequestDTO request)
    {
        
        
        var normalizedPhone = PhoneNormalizer.Normalize(request.Phone);

        var errors = PhoneNumberValidator.IsValid(normalizedPhone,request);

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
        
        var client = await _clientRepository.GetByPhoneAsync(normalizedPhone);

        if (client == null)
        {
            throw new NotFoundException("This client doesn't exist");
        }

        var clientDto = new ClientResponseDTO
        {
            Id = client.id,
            Name = client.Name,
            Phone = client.Phone,
            Addresses = client.Addresses.Select(a => new AddressSumaryDTO
            {
                Id = a.Id,
                Cep = a.Cep,
                StreetName = a.StreetName,
                HouseNumber = a.HouseNumber,
                Description = a.Description,
                NeighborhoodId = a.NeighborhoodId,
                NeighborhoodName = a.Neighborhood.Name,
                DeliveryFee = a.Neighborhood.DeliveryFee
            }).ToList(),
            Email = client.Email,
        };
        
        return clientDto;
    }

    public async Task<ClientResponseDTO> CreateClient(ClientRequestDTO request)
    {
        
        string normalizedPhone = PhoneNormalizer.Normalize(request.Phone);
        
        var exist = await _clientRepository.GetByPhoneAsync(request.Phone);

        if (exist != null)
        {
            var existErrors = new List<ValidationError>();
            
            existErrors.Add(new ValidationError
            {
                Field = nameof(request.Phone),
                Message = "This client already exists"
            });
            
            throw new ValidationException(existErrors,409);
        }

        var errors =  ClientValidator.IsValid(request,normalizedPhone);

       if (errors.Any())
       {
           throw new ValidationException(errors);
       }

       var client = new Client
       {
           Phone = normalizedPhone,
           Name = request.Name!,
           Email = request.Email,
           CreatedAt = DateTime.Now
       };

       await _clientRepository.AddAsync(client);

        var clientDto = new ClientResponseDTO
        {
            Phone = client.Phone,
            Name = client.Name,
            Email = client.Email,
            Id =  client.id,
            Addresses = client.Addresses.Select(a => new AddressSumaryDTO
            {
                Id = a.Id,
                Cep = a.Cep,
                StreetName = a.StreetName,
                HouseNumber = a.HouseNumber,
                Description = a.Description,
                NeighborhoodId = a.NeighborhoodId,
                NeighborhoodName = a.Neighborhood.Name,
                DeliveryFee = a.Neighborhood.DeliveryFee
            }).ToList(),
        };
        
        return clientDto;
    }

    public async Task<ClientResponseDTO> UpdateClient(int id,ClientUpdateRequestDTO request)
    {
        var client = await _clientRepository.GetByIdAsync(id);

        if (client == null)
        {
            throw new NotFoundException("This client doesn't exist");
        }
        
        
        if (request.Phone is not null)
        {
            var normalizedPhone = PhoneNormalizer.Normalize(request.Phone);

            var errors = PhoneNumberValidator.IsValidForUpdate(normalizedPhone);

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            
            var exist = await _clientRepository.GetByPhoneAsync(normalizedPhone);

            if (exist is not null && exist.id != client.id)
            {
                
                throw new ValidationException([
                    new ValidationError()
                    {
                        Field = nameof(request.Phone),
                        Message = "This client already exists"
                    }
                ],409);
            }
            
            client.Phone = normalizedPhone;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            client.Name = request.Name.Trim();
        }
        
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var email = request.Email.Trim();

            if (!EmailValidator.IsValid(request.Email)) throw new ValidationException([ new ValidationError
            {
                Field = nameof(request.Email),
                Message = "This Email isn't a valid email"
            }]);
            
            client.Email = request.Email;
        }


        await _clientRepository.PatchAsync(client);

        var clientDto = new ClientResponseDTO
        {
            Id = client.id,
            Name = client.Name,
            Phone = client.Phone,
            Addresses = client.Addresses.Select(a => new AddressSumaryDTO
            {
                Id = a.Id,
                Cep = a.Cep,
                StreetName = a.StreetName,
                HouseNumber = a.HouseNumber,
                Description = a.Description,
                NeighborhoodId = a.NeighborhoodId,
                NeighborhoodName = a.Neighborhood.Name,
                DeliveryFee = a.Neighborhood.DeliveryFee
            }).ToList(),
            Email = client.Email,
        };
        
        return clientDto;
    }
}