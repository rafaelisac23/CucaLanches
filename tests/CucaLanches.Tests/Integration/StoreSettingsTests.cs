using System.Net.Http.Json;
using CucaLanches.Application.StoreSettings.DTOs;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Tests.Integration;

public class StoreSettingsTests:IClassFixture<DatabaseTestFactory>
{
    private readonly HttpClient _client;

    public StoreSettingsTests(DatabaseTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]

    public async Task Toggle_changes_public_status()
    {

        await _client.PatchAsJsonAsync("/store/status", new {
            isOpen = true
        });

        var test1 = await _client.GetFromJsonAsync<StoreSettingsResponseDTO>("/store/status");
        
        Assert.True(test1!.IsOpen);
        
        await _client.PatchAsJsonAsync("/store/status", new {
            isOpen = false
        });

        var test2 = await _client.GetFromJsonAsync<StoreSettingsResponseDTO>("/store/status");
        
        Assert.False(test2!.IsOpen);

    }
    
}