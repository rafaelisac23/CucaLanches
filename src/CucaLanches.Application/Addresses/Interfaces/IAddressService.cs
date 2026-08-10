using CucaLanches.Application.Addresses.DTOs;

namespace CucaLanches.Application.Addresses.Interfaces;

public interface IAddressService
{
    public  Task<AddressResponseDTO> AddAsync(AddressRequestDTO request);
    
    public Task<List<AddressSumaryDTO>>  GetAllAsync();
    
    public Task<AddressSumaryDTO> GetByIdAsync(int id);
    
    public Task<AddressSumaryDTO> PatchAsync(int id, AddressRequestPatchDTO request);
    
    public Task<AddressSumaryDTO> RemoveAsync(int id);
}