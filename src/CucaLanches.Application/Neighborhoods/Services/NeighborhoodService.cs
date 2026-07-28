using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Neighborhoods.DTOs;
using CucaLanches.Application.Neighborhoods.Interfaces;
using CucaLanches.Application.Validators;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Neighborhoods.Services;

public class NeighborhoodService:INeighborhoodService
{
    
    private readonly INeighborhoodRepository _repository;

    public NeighborhoodService(INeighborhoodRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<NeighborhoodResponseDTO> CreateAsync(NeighborhoodRequestDTO request)
    {
        var errors = NeighborhoodValidator.IsValid(request);
        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
        
        var exist = await _repository.GetByName(request.Name);

        if (exist != null)
        {
            throw new ValidationException(new List<ValidationError>()
            {
                new()
                {
                    Field =  "Name",
                    Message = "Name is already in use"
                }
            },409);
        }
        
        
        
        var neighborhood = new Neighborhood
        {
            Name = request.Name,
            DeliveryFee = request.DeliveryFee,
            IsAvaliable = request.IsAvaible
        };
        
        await _repository.AddAync(neighborhood);

        var response = new NeighborhoodResponseDTO
        {
            Id = neighborhood.Id,
            Name = neighborhood.Name,
            DeliveryFee = neighborhood.DeliveryFee,
            IsAvaible = neighborhood.IsAvaliable
        };
        
        return response;
    }

    public async Task<List<NeighborhoodResponseDTO>> ListAsync(bool all)
    {
        var neighborhoods = new List<Neighborhood>();

        if (all == true)
        {
            neighborhoods = await _repository.GetAllAsync();
        }
        else
        {
            neighborhoods = await _repository.GetAllExceptInactiveAsync();
        }

        if (!neighborhoods.Any())
        {
            throw new NotFoundException("Neighborhoods not found");
        }

        var responseDto = neighborhoods.Select(n => new NeighborhoodResponseDTO
        {
            Id = n.Id,
            Name = n.Name,
            DeliveryFee = n.DeliveryFee,
            IsAvaible = n.IsAvaliable
        });

        return responseDto.ToList();

    }

    public async Task<NeighborhoodUpdatedResponseDTO> UpdateAsync(int id,NeighborhoodUpdateRequestDTO request)
    {
        var neighborhood = await _repository.GetById(id);

        if (neighborhood == null)
        {
            throw new NotFoundException("Neighborhood not found");
        }
        
        var errors = NeighborhoodValidator.IsValidForUpdate(request);

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
        

        if (request.Name is not null)
        {
            var exists = await _repository.GetByName(request.Name);

            if (exists != null && exists.Id != id)
            {
                throw new ValidationException(new List<ValidationError>
                {
                    new()
                    {
                        Field = nameof(request.Name),
                        Message = "A neighborhood with this name already exists."
                    }
                },409);
            }

            neighborhood.Name = request.Name;
        }

        if (request.DeliveryFee.HasValue)
        {
            neighborhood.DeliveryFee = request.DeliveryFee.Value;
        }

        if (request.IsAvaible.HasValue)
        {
            neighborhood.IsAvaliable = request.IsAvaible.Value;
        }
        
        await _repository.UpdateAync(neighborhood);

        var updatedNeighborhood = new NeighborhoodUpdatedResponseDTO
        {
            Id = neighborhood.Id,
            Name = neighborhood.Name,
            DeliveryFee = neighborhood.DeliveryFee,
            IsAvaible = neighborhood.IsAvaliable
        };

        return updatedNeighborhood;
    }
}