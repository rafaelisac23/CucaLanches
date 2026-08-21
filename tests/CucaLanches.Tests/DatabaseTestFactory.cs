using System.Data.Common;
using CucaLanches.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Respawn;
using Respawn.Graph;
using Xunit;

namespace CucaLanches.Tests;

public class DatabaseTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private string _connectionString = string.Empty;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices((context, services) =>
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection")!;

            // 1. Remove o DbContext original
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // 2. Configura o DbContext para os testes com MySQL
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // 3. Garante que as tabelas sejam criadas no Docker
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.EnsureCreated();
        });
    }

    public async Task InitializeAsync()
    {
        // FORÇA a criação do Host do .NET. Isso executa o ConfigureWebHost 
        // e carrega o IConfiguration a partir do appsettings.Testing.json
        var configuration = Services.GetRequiredService<IConfiguration>();
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        // Agora a _connectionString possui o valor correto!
        _dbConnection = new MySqlConnection(_connectionString);
        await _dbConnection.OpenAsync();

        // Configura o Respawn para o MySQL
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.MySql,
            TablesToIgnore = new Table[]
            {
                "__EFMigrationsHistory"
            }
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public new async Task DisposeAsync()
    {
        if (_dbConnection != null)
        {
            await _dbConnection.CloseAsync();
            await _dbConnection.DisposeAsync();
        }
        await base.DisposeAsync();
    }
}