using CucaLanches.Application.StoreSettings.DTOs;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.StoreSettings.Interfaces;

public interface IStoreSettingService
{
    public Task<StoreSettingsResponseDTO> Get();
    public Task<StoreSettingsResponseDTO> Patch( StoreSettingsRequestDTO isOpen);
}