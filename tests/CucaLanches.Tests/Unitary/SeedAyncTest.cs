

using CucaLanches.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CucaLanches.Tests.Unitary;

public class SeedAyncTest:IClassFixture<DatabaseTestFactory>
{
    private readonly DatabaseTestFactory _factory;
    
    public SeedAyncTest(DatabaseTestFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task Verify_test_add_twice()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetService<AppDbContext>();

        await DataSeeder.SeedAsync(db);
        await DataSeeder.SeedAsync(db);

        Assert.Single(db.Users.Where(u => u.Email == "Admin@gmail.com"));
        Assert.Single(db.StoreSettings);
        Assert.True(db.Neighborhoods.Any() && db.Products.Any());
    }
}