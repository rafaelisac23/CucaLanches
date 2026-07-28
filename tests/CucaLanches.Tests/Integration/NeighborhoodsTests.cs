
using System.Net;
using System.Net.Http.Json;
using CucaLanches.Application.Neighborhoods.DTOs;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Tests.Integration;

public class NeighborhoodsTests:IClassFixture<DatabaseTestFactory>
{
    
    private readonly HttpClient _Client;

    public NeighborhoodsTests(DatabaseTestFactory factory)
    {
        _Client = factory.CreateClient();
    }

    [Fact]
    public async Task Duplicate_Name_returns_409()
    {
        await _Client.PostAsJsonAsync("/Neighborhood", new  {
            id = 0,
            name = "string",
            deliveryFee = 0,
            isAvaible = true
        });
        var dup = await _Client.PostAsJsonAsync("/Neighborhood", new  {
            id = 0,
            name = "string",
            deliveryFee = 0,
            isAvaible = true
        });
        
        Assert.Equal(HttpStatusCode.Conflict, dup.StatusCode);
    }

    [Fact]
    public async Task Public_list_hides_unavailable()
    {
        
        await _Client.PostAsJsonAsync("/Neighborhood", new  {
            name = "Jardim do vale",
            deliveryFee = 0,
            isAvaible = true
        });
        var addedinactiveNeighborhood = await _Client.PostAsJsonAsync("/Neighborhood", new  {
            name = "Jardim primavera",
            deliveryFee = 0,
            isAvaible = false
        });
        Assert.Equal(HttpStatusCode.OK, addedinactiveNeighborhood.StatusCode);
        
        var inactiveNeighborhood = await addedinactiveNeighborhood.Content.ReadFromJsonAsync<Neighborhood>();
        
        var list = await _Client.GetFromJsonAsync<List<NeighborhoodResponseDTO>>("/Neighborhood");
        var adminList = await _Client.GetFromJsonAsync<List<NeighborhoodResponseDTO>>("/Neighborhood?all=true");
        
        Assert.DoesNotContain(list, n => n.Id == inactiveNeighborhood.Id);
        Assert.Contains(adminList, n => n.Id == inactiveNeighborhood.Id);
    }

    [Fact]
    public async Task Negative_fee_returns_400()
    {
        
        var NeighborhoodResponse = await _Client.PostAsJsonAsync("/Neighborhood", new  {
            name = "JK",
            deliveryFee = -20,
            isAvaible = true
        });
        
        Assert.Equal(HttpStatusCode.BadRequest, NeighborhoodResponse.StatusCode);
        
    }
}