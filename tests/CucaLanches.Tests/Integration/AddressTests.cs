using System.Net;
using System.Net.Http.Json;
using CucaLanches.Application.Addresses.DTOs;
using CucaLanches.Domain.Entities;

namespace CucaLanches.Tests.Integration;

public class AddressTests:BaseIntegrationTest
{
    
    public AddressTests(DatabaseTestFactory factory) : base(factory)
    {
    }


    [Fact]
    public async Task When_request_and_found_a_avoidArray_return_404()
    {
        var addresses = await Client.GetAsync("/api/Address");
        
        Assert.Equal(HttpStatusCode.NotFound, addresses.StatusCode);
    }
    
    [Fact]
    public async Task If_send_a_post_with_Invalid_Client_return_404()
    {
        var address = await Client.PostAsJsonAsync("/api/Address", new {
            clientId= 99,
            neighborhoodId= 1,
            cep= "12519160",
            streetName= "aaa",
            houseNumber= 20,
            description= "djeidioqwjd"
        });
        
        Assert.Equal(HttpStatusCode.NotFound, address.StatusCode);
    }
    
    
}