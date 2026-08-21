using System.Net.Http.Json;
using CucaLanches.Application.StoreSettings.DTOs;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Tests.Integration;

public class StoreSettingsTests:BaseIntegrationTest
{
    
    public StoreSettingsTests(DatabaseTestFactory factory) : base(factory)
    {
    }

    [Fact]

    public async Task Toggle_changes_public_status()
    {

        await Client.PatchAsJsonAsync("/store/status", new {
            isOpen = true
        });

        var test1 = await Client.GetFromJsonAsync<StoreSettingsResponseDTO>("/store/status");
        
        Assert.True(test1!.IsOpen);
        
        await Client.PatchAsJsonAsync("/store/status", new {
            isOpen = false
        });

        var test2 = await Client.GetFromJsonAsync<StoreSettingsResponseDTO>("/store/status");
        
        Assert.False(test2!.IsOpen);

    }
    
}