using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CucaLanches.Infrastructure.DependencyInjection;

public static class InfraestructureServiceCollection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    { 
        string Connection = configuration.GetConnectionString("DefaultConnection") 
                            ?? throw new InvalidOperationException("No connection string");
        
        services.AddDbContext<AppDbContext>(options => options.UseMySql(Connection,ServerVersion.AutoDetect(Connection)));
        
        return services;
    }
    
}