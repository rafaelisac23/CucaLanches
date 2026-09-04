using System.Text;
using CucaLanches.Api.Middlewares;
using CucaLanches.Application.Addresses.Interfaces;
using CucaLanches.Application.Addresses.Services;
using CucaLanches.Application.Auth.Interfaces;
using CucaLanches.Application.Auth.Services;
using CucaLanches.Application.Clients.Interfaces;
using CucaLanches.Application.Clients.Services;
using CucaLanches.Application.Common;
using CucaLanches.Application.Exceptions;
using CucaLanches.Application.Neighborhoods.Interfaces;
using CucaLanches.Application.Neighborhoods.Services;
using CucaLanches.Application.Orders.Interfaces;
using CucaLanches.Application.Orders.Services;
using CucaLanches.Application.Products.Interfaces;
using CucaLanches.Application.Products.Services;
using CucaLanches.Application.PublicMenu.Interfaces;
using CucaLanches.Application.PublicMenu.Services;
using CucaLanches.Application.StoreSettings.Interfaces;
using CucaLanches.Application.StoreSettings.Services;
using CucaLanches.Application.Users.Interfaces;
using CucaLanches.Infrastructure;
using CucaLanches.Infrastructure.Addresses;
using CucaLanches.Infrastructure.Clients;
using CucaLanches.Infrastructure.DependencyInjection;
using CucaLanches.Infrastructure.Neighborhoods;
using CucaLanches.Infrastructure.Orders;
using CucaLanches.Infrastructure.Products;
using CucaLanches.Infrastructure.PublicMenu;
using CucaLanches.Infrastructure.StoreSettings;
using CucaLanches.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<ITokenService,TokenService>();


//Auth

var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new NotFoundException("problem jwt:Key in Program.cs ");
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnly", policy => policy.RequireClaim("type", "user"));
    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("type", "client"));
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("type", "user").RequireRole("Admin"));
});
    


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

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
