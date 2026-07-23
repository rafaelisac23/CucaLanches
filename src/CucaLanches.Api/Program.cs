using CucaLanches.Api.Middlewares;
using CucaLanches.Application.Products.Interfaces;
using CucaLanches.Application.Products.Services;
using CucaLanches.Application.PublicMenu.Interfaces;
using CucaLanches.Application.PublicMenu.Services;
using CucaLanches.Infrastructure.DependencyInjection;
using CucaLanches.Infrastructure.Products;
using CucaLanches.Infrastructure.PublicMenu;

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

app.Run();

public partial class Program { }
