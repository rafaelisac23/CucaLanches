using CucaLanches.Application.StoreSettings.DTOs;
using CucaLanches.Application.StoreSettings.Interfaces;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.StoreSettings.Services;

public class StoreSettingService:IStoreSettingService
{
    
    private readonly IStoreSettingRepository _repository;

    public StoreSettingService(IStoreSettingRepository  repository)
    {
        _repository = repository;
    }
    
    
    private async Task<StoreSetting> GetRow()
    {
        var storeSetting = await _repository.Get();

        if (storeSetting == null)
        {
            var newStoreSetting = new StoreSetting
            {
                IsOpen = true
            };

           await  _repository.Add(newStoreSetting);
            
            return newStoreSetting;
        }
        
        return storeSetting;
    }

    public async Task<StoreSettingsResponseDTO> Get()
    {
        var storeSetting = await GetRow();

        var storeSettingDto = new StoreSettingsResponseDTO
        {
          
            IsOpen = storeSetting.IsOpen
        };
        
        return storeSettingDto;
    }

    public async Task<StoreSettingsResponseDTO> Patch(StoreSettingsRequestDTO request)
    {
        var storeSetting = await GetRow();

        storeSetting.IsOpen = request.IsOpen;

        await _repository.Patch(storeSetting);

        var storeSettingDto = new StoreSettingsResponseDTO
        {
            IsOpen = storeSetting.IsOpen
        };

        return storeSettingDto;
    }
}