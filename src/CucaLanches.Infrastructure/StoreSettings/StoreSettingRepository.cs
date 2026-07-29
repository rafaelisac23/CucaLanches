using CucaLanches.Application.StoreSettings.Interfaces;
using CucaLanches.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CucaLanches.Infrastructure.StoreSettings;

public class StoreSettingRepository:IStoreSettingRepository
{
    private readonly AppDbContext _dbContext;

    public StoreSettingRepository(AppDbContext  dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<StoreSetting?> Get()
    {
        return await _dbContext.StoreSettings.FirstOrDefaultAsync();
    }

    public async Task Add(StoreSetting storeSetting)
    {
       _dbContext.StoreSettings.Add(storeSetting);
        await  _dbContext.SaveChangesAsync();
    }

    public async Task Patch(StoreSetting storeSetting)
    {
        _dbContext.StoreSettings.Update(storeSetting);
        await _dbContext.SaveChangesAsync();
    }
}