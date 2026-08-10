using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.Addresses.Interfaces;

public interface IAddressRepository
{
    Task<List<Address>> GetAll();
    Task<Address?> GetByIdAsync(int id);
    Task AddAsync(Address address);
    Task PatchAsync(Address address);
    Task DeleteAsync(Address address);
    
}