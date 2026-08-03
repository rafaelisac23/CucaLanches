using System.Net;
using System.Net.Http.Json;
using CucaLanches.Application.Clients.DTOs;

namespace CucaLanches.Tests.Integration;

public class ClientTests:IClassFixture<DatabaseTestFactory>
{

    private readonly HttpClient _client;

    public ClientTests(DatabaseTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task New_phone_requires_name_and_creates()
    {

        var offName = await _client.PostAsJsonAsync("/api/Client", new { phone = "5512991903030" });
        
        Assert.Equal(HttpStatusCode.BadRequest, offName.StatusCode);

        var newUser = await _client.PostAsJsonAsync("/api/Client", new {
            name  = "Maria",
            phone = "55 (12)991903030",
        });
        
        var bodyNewUser = await newUser.Content.ReadFromJsonAsync<ClientResponseDTO>();
        
        Assert.Equal(HttpStatusCode.OK, newUser.StatusCode);
        Assert.Equal("5512991903030",bodyNewUser!.Phone);
        
    }

    [Fact]
    public async Task Invalid_phone_returns_400()
    {
         var withInvalidNumber = await _client.PostAsJsonAsync("/api/Client", new {
            name  = "X",
            phone = "123",
        });
         
         Assert.Equal(HttpStatusCode.BadRequest, withInvalidNumber.StatusCode);
    }
    
}