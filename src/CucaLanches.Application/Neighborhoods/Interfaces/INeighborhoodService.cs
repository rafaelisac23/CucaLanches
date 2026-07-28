using CucaLanches.Application.Neighborhoods.DTOs;

namespace CucaLanches.Application.Neighborhoods.Interfaces;

public interface INeighborhoodService
{
    Task<List<NeighborhoodResponseDTO>> ListAsync(bool all);
    Task<NeighborhoodResponseDTO> CreateAsync(NeighborhoodRequestDTO neighborhood);
    Task<NeighborhoodUpdatedResponseDTO> UpdateAsync(int id,NeighborhoodUpdateRequestDTO request);
}