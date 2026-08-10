using CucaLanches.Application.Addresses.DTOs;
using CucaLanches.Application.Addresses.Interfaces;
using CucaLanches.Application.Clients.Interfaces;
using CucaLanches.Application.Common;
using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Neighborhoods.Interfaces;
using CucaLanches.Application.Validators;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Addresses.Services;

public class AddressService:IAddressService
{
    
    private readonly IAddressRepository _addressRepository;
    private readonly IClientRepository _clientRepository;
    private readonly INeighborhoodRepository _neighborhoodRepository;

    public AddressService(IAddressRepository addressRepository, IClientRepository clientRepository, INeighborhoodRepository neighborhoodRepository)
    {
        _addressRepository = addressRepository;
        _clientRepository = clientRepository;
        _neighborhoodRepository = neighborhoodRepository;
    }



    public async Task<List<AddressSumaryDTO>> GetAllAsync()
    {
        var addresses = await _addressRepository.GetAll();
        
        if(!addresses.Any()) throw new NotFoundException("No addresses found");

        var addressesDto = addresses.Select(address => new AddressSumaryDTO
        {
            Id = address.Id,
            Cep =  address.Cep,
            StreetName = address.StreetName,
            HouseNumber = address.HouseNumber,
            Description = address.Description,
            NeighborhoodId = address.NeighborhoodId,
            DeliveryFee = address.Neighborhood.DeliveryFee,
            NeighborhoodName = address.Neighborhood.Name
        }).ToList();

        return addressesDto;
    }

    public async Task<AddressSumaryDTO> GetByIdAsync(int id)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        
        if(address is null) throw new NotFoundException("Address does not exist");

        var addressDto = new AddressSumaryDTO
        {
            Id = address.Id,
            Cep = address.Cep,
            StreetName = address.StreetName,
            HouseNumber = address.HouseNumber,
            Description = address.Description,
            NeighborhoodId = address.NeighborhoodId,
            NeighborhoodName = address.Neighborhood.Name,
            DeliveryFee = address.Neighborhood.DeliveryFee,
        };
        
        return addressDto;
    }
    
    public async Task<AddressResponseDTO> AddAsync(AddressRequestDTO request)
    {

        if (request.Cep is not null)
        {
            var normalizedCep = CepNormalizer.Normalize(request.Cep);
        
            request.Cep = normalizedCep;
        }
        
        var errors = AddressValidator.IsValid(request);
        
        if(errors.Any()) throw new ValidationException(errors);

        var clientId = request.ClientId!.Value;
        var neighborhoodId = request.NeighborhoodId!.Value;
        
        var client = await _clientRepository.GetByIdAsync(clientId);
        var neighborhood = await _neighborhoodRepository.GetById(neighborhoodId);
        
        if(client is null) throw new NotFoundException("Client does not exist");
        if(neighborhood is null) throw new NotFoundException("Neighborhood does not exist");

        var newAddress = new Address
        {
            ClientId = clientId,
            NeighborhoodId = neighborhoodId,
            Cep = request.Cep!,
            StreetName = request.StreetName!.Trim(),
            HouseNumber = request.HouseNumber!.Value,
            Description = request.Description
        };
        
        await _addressRepository.AddAsync(newAddress);

        var newAddressDto = new AddressResponseDTO
        {
            Id =  newAddress.Id,
            ClientId = newAddress.ClientId,
            ClientName = client.Name,
            NeighborhoodId = newAddress.NeighborhoodId,
            NeighborhoodName = neighborhood.Name,
            DeliveryFee = neighborhood.DeliveryFee,
            Cep = newAddress.Cep,
            StreetName = newAddress.StreetName,
            HouseNumber = newAddress.HouseNumber,
            Description = newAddress.Description
        };
        return newAddressDto;
    }

    public async Task<AddressSumaryDTO> PatchAsync(int id, AddressRequestPatchDTO request)
    {
        var address = await _addressRepository.GetByIdAsync(id);
        
        if(address is null) 
            throw new NotFoundException("Address does not exist");

        if (request.Cep is not null)
        {
            request.Cep = CepNormalizer.Normalize(request.Cep);
        }
        
        var errors = AddressValidator.IsValidForUpdate(request);
        
        if(errors.Any()) throw new ValidationException(errors);
        
        if (request.NeighborhoodId.HasValue)
        {
            var neighborhoodExists = await _neighborhoodRepository.ExistAsync(request.NeighborhoodId!.Value);
            
            if(!neighborhoodExists) 
                throw new NotFoundException("Neighborhood does not exist");
            
            address.NeighborhoodId =  request.NeighborhoodId.Value;
        }

        if (request.StreetName is not null)
            address.StreetName = request.StreetName;
        

        if (request.Cep is not null)
            address.Cep = request.Cep;
        

        if (request.HouseNumber.HasValue)
            address.HouseNumber = request.HouseNumber.Value;
        

        if (request.Description is not null)
            address.Description = request.Description;
        
        
        await _addressRepository.PatchAsync(address);
        
        address = await _addressRepository.GetByIdAsync(id);
        
        if(address is null) throw new NotFoundException("Address does not exist");

        return  new AddressSumaryDTO
        {
            Id =   address.Id,
            Cep = address.Cep,
            StreetName = address.StreetName,
            HouseNumber = address.HouseNumber,
            NeighborhoodId = address.NeighborhoodId,
            NeighborhoodName = address.Neighborhood.Name,
            DeliveryFee = address.Neighborhood.DeliveryFee,
            Description = address.Description
        };

    }

    public async Task<AddressSumaryDTO> RemoveAsync(int id)
    {
        var address = await _addressRepository.GetByIdAsync(id);

        if (address is null) throw new NotFoundException("This Address don't exist");

        await _addressRepository.DeleteAsync(address);

        return new AddressSumaryDTO
        {
            Id = address.Id,
            Cep = address.Cep,
            StreetName = address.StreetName,
            HouseNumber = address.HouseNumber,
            Description = address.Description,
            NeighborhoodId = address.Neighborhood.Id,
            NeighborhoodName = address.Neighborhood.Name,
            DeliveryFee = address.Neighborhood.DeliveryFee
        };

    }
    
}