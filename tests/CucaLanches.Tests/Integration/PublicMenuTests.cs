using System.Net;
using System.Net.Http.Json;
using CucaLanches.Application.Products.DTOs;
using CucaLanches.Application.PublicMenu.DTOs;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Tests.Integration;

public class PublicMenuTests:BaseIntegrationTest
{
    
    public PublicMenuTests(DatabaseTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Menu_shows_only_active_grouped_by_type()
    {
        var product1 = new ProductRequestDTO
        {
            Name = "Produto Teste",
            Description = "Teste",
            Type = ProductType.Drink,
            Price = 1.10m
        };
        
        var product2 = new ProductRequestDTO
        {
            Name = "Produto Teste",
            Description = "Teste",
            Type = ProductType.Drink,
            Price = 1.10m
        };
        
        
        var newProduct = await Client.PostAsJsonAsync("/Product", product1);
        var newProduct2 = await Client.PostAsJsonAsync("/Product", product2);
        
        
        Assert.Equal(HttpStatusCode.OK, newProduct.StatusCode);

        var productinformations = await newProduct.Content.ReadFromJsonAsync<ProductResponseDTO>();
        
        var desactivateProduct = await Client.DeleteAsync($"/Product/{productinformations.Id}");
        
        Assert.Equal(HttpStatusCode.OK, desactivateProduct.StatusCode);
        
        var GetAll = await Client.GetFromJsonAsync<List<PublicMenuResponseDTO>>("/PublicMenu");

        var all = GetAll.SelectMany(group => group.Products);
        
        Assert.DoesNotContain(all, x => x.Id == productinformations.Id);
        
        Assert.All(all, p => Assert.True(p.Active));

    }
    
}