

using System.Net;
using System.Net.Http.Json;
using CucaLanches.Application.Products.DTOs;
using CucaLanches.Application.Products.Interfaces;
using CucaLanches.Domain.Entities;
using CucaLanches.Domain.Enums;
using CucaLanches.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CucaLanches.Tests.Integration;

public class ProductTests:BaseIntegrationTest
{
    
    public ProductTests(DatabaseTestFactory factory) : base(factory)
    {
    }


    [Fact]
    public async Task Verify_response_created_200()
    {
        var response = await Client.PostAsJsonAsync("/Product", new {
            name = "Hamburguer",
            type = 1,
            description = "teste do ProductsTest",
            price= 10
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task Post_Price_0_returns_400()
    {
        var response = await Client.PostAsJsonAsync("/Product", new {
            name = "Hamburguer",
            type = 1,
            description = "teste do ProductsTest",
            price= 0
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Get_Unknown_id_returns_404()
    {
        var response = await Client.GetAsync("/Product/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task Delete_is_logical()
    {

        var request = new ProductRequestDTO
        {
            Name = "Um hamburguer de teste",
            Description = "Uma descrição de teste",
            Price = 10,
            Type = ProductType.Lunch
        };
        
        var response = await Client.PostAsJsonAsync("/Product", request);
        
        //Arrange
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var newProduct = await response.Content.ReadFromJsonAsync<ProductResponseDTO>();
        
        //Act

         var deletedProduct =  await Client.DeleteAsync($"/Product/{newProduct.Id}");
         
         Assert.Equal(HttpStatusCode.OK, deletedProduct.StatusCode);
         
        //ASSERT
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var product = await db.Products.FindAsync(newProduct.Id);
        
        Assert.NotNull(product);
        Assert.False(product!.Active);
    }
}