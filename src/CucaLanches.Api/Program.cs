using CucaLanches.Api.Middlewares;
using CucaLanches.Application.Addresses.Interfaces;
using CucaLanches.Application.Addresses.Services;
using CucaLanches.Application.Clients.Interfaces;
using CucaLanches.Application.Clients.Services;
using CucaLanches.Application.Neighborhoods.Interfaces;
using CucaLanches.Application.Neighborhoods.Services;
using CucaLanches.Application.Products.Interfaces;
using CucaLanches.Application.Products.Services;
using CucaLanches.Application.PublicMenu.Interfaces;
using CucaLanches.Application.PublicMenu.Services;
using CucaLanches.Application.StoreSettings.Interfaces;
using CucaLanches.Application.StoreSettings.Services;
using CucaLanches.Infrastructure;
using CucaLanches.Infrastructure.Addresses;
using CucaLanches.Infrastructure.Clients;
using CucaLanches.Infrastructure.DependencyInjection;
using CucaLanches.Infrastructure.Neighborhoods;
using CucaLanches.Infrastructure.Products;
using CucaLanches.Infrastructure.PublicMenu;
using CucaLanches.Infrastructure.StoreSettings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB Connection
builder.Services.AddInfrastructure(builder.Configuration);

//Controllers Services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPublicMenuService, PublicMenuService>();
builder.Services.AddScoped<IPublicMenuRepository, PublicMenuRepository>();
builder.Services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
builder.Services.AddScoped<INeighborhoodService,NeighborhoodService>();
builder.Services.AddScoped<IStoreSettingService, StoreSettingService>();
builder.Services.AddScoped<IStoreSettingRepository, StoreSettingRepository>();
builder.Services.AddScoped<IClientService,ClientService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService,AddressService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
    await DataSeeder.SeedAsync(db);
}

app.Run();

public partial class Program { }
