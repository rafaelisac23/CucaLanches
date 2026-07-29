using CucaLanches.Domain.Entities;

namespace CucaLanches.Application.StoreSettings.Interfaces;

public interface IStoreSettingRepository
{
    Task<StoreSetting?> Get();
    Task Add(StoreSetting storeSetting);
    Task Patch(StoreSetting storeSetting);
}