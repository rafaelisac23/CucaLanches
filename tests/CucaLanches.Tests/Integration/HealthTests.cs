using System.Net;
using CucaLanches.Tests;
using CucaLanches.Tests.Integration;


public class HealthTests:BaseIntegrationTest
{
    
    public HealthTests(DatabaseTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Health_return_Ok200()
    {
        var response = await Client.GetAsync("health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
}