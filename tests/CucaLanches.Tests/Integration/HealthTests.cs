using System.Net;

namespace CucaLanches.Tests.Integration;

public class HealthTests:IClassFixture<IntegrationFactory>
{
    private readonly HttpClient _client;

    public HealthTests(IntegrationFactory factory)
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