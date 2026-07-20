using CucaLanches.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CucaLanches.Tests;

/*Criei essa classe para fazer com que ele modifique o padrão de serviços do meu projeto para que possa ser usado
 no meu projeto de testes
*/
public class DatabaseTestFactory:WebApplicationFactory<Program>//Aqui ele herda pq o .net ja sabe construir a aplicação,so quero alterala
{
    
    private readonly SqliteConnection _connection = new ("DataSource=:memory:");
    
    protected override void ConfigureWebHost(IWebHostBuilder builder) //como nao sou eu que chamo esse método e sim o webApplicationFactory estou reescrevendo ele
        {
            _connection.Open(); // cria a conexão com o banco dosqlite
 
            //Como quero acessar e modificar os serviços da minha aplicação, vou acessar a area de serviços
            //o padrão da microsoft disponibiliza um metodo para que eu acesse essa área

            builder.ConfigureServices(services =>
            {

                builder.UseEnvironment("Testing");

                var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
   
                //remove do IServiceCollection 
                if(descriptor != null) services.Remove(descriptor);

                //cria uma nova receita de banco usando sqliteinmemory
                services.AddDbContext<AppDbContext>(options => options.UseSqlite(_connection));
   
                //instancia um scope  
                var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                //cria o banco 
                context.Database.EnsureCreated();
            });
  
        }
 
        protected override void Dispose(bool disposing)
        {
            _connection.Dispose();
            base.Dispose(disposing);
        }
    
}