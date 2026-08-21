using Xunit;

namespace CucaLanches.Tests.Integration;

public abstract class BaseIntegrationTest : IClassFixture<DatabaseTestFactory>, IAsyncLifetime
{
    protected readonly DatabaseTestFactory Factory;
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(DatabaseTestFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    // Executa ANTES de cada [Fact] ou [Theory] da classe
    public async Task InitializeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}