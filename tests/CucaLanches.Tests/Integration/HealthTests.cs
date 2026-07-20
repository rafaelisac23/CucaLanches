using System.Net;
using CucaLanches.Tests;


public class HealthTests:IClassFixture<DatabaseTestFactory>
{
    private readonly HttpClient _client;

    public HealthTests(DatabaseTestFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]

    public async Task Health_return_Ok200()
    {
        var response = await _client.GetAsync("health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
}