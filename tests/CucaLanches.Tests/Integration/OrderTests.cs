using System.Net;
using System.Net.Http.Json;
using CucaLanches.Application.Addresses.DTOs;
using CucaLanches.Application.Clients.DTOs;
using CucaLanches.Application.Neighborhoods.DTOs;
using CucaLanches.Application.Orders.DTOs;
using CucaLanches.Application.Products.DTOs;
using CucaLanches.Domain.Entities;
using Xunit.Abstractions;

namespace CucaLanches.Tests.Integration;


public class OrderTests:BaseIntegrationTest
{
    
    public OrderTests(DatabaseTestFactory factory) : base(factory)
    {
    }


    [Fact]
    public async Task First_order_on_day_ConcurrentRequests_ShouldAssignSequentialNumbersWithoutErrors()
    {

       var openResponse =  await Client.PatchAsJsonAsync("/store/status", new { isOpen = true });
       
       Assert.Equal(HttpStatusCode.OK, openResponse.StatusCode);

       var newJsonProduct = new
       {
           name = "coca cola geladinha",
           type = 0,
           description = "uma coquinha trincando",
           price = 8
       };
       
       var newProduct = await  Client.PostAsJsonAsync("/Product", newJsonProduct);
       
       Assert.Equal(HttpStatusCode.OK, newProduct.StatusCode);

       
       var newJsonCLient = new
       {
           name = "Rafael isac",
           phone = "5512991901969",
           email = "rafaeldarrigo@gmail.com"
       };
       
       var newCLient = await  Client.PostAsJsonAsync("/api/Client", newJsonCLient);
       
       Assert.Equal(HttpStatusCode.OK, newCLient.StatusCode);

       var newJsonNeighborhood = new
       {
           name = "josé bonifacio",
           deliveryFee = 10,
           isAvaible = true
       };
       
       var newNeighborhood = await Client.PostAsJsonAsync("/Neighborhood", newJsonNeighborhood);
       
       Assert.Equal(HttpStatusCode.OK, newNeighborhood.StatusCode);
       
       var newNeighborhoodResponse = await newNeighborhood.Content.ReadFromJsonAsync<NeighborhoodResponseDTO>();
       var newClientResponse = await newCLient.Content.ReadFromJsonAsync<ClientResponseDTO>();
       
       // ADICIONE ESTES CONSOLE.WRITELINE PARA DIAGNOSTICAR:
       Console.WriteLine($"Client ID retornado: {newClientResponse?.Id}");
       Console.WriteLine($"Neighborhood ID retornado: {newNeighborhoodResponse?.Id}");
       
       
       var newProductResponse = await newProduct.Content.ReadFromJsonAsync<ProductResponseDTO>();
       
       var newJsonAddress = new
       {
           clientId = newClientResponse!.Id,
           neighborhoodId = newNeighborhoodResponse!.Id,
           cep = "12519160",
           streetName = "rua tupinamba",
           houseNumber = 10,
           description = "string"
       };

       var newAddress = await Client.PostAsJsonAsync("/api/Address", newJsonAddress);
       Assert.Equal(HttpStatusCode.OK, newAddress.StatusCode);
       
       var newAddressResponse = await newAddress.Content.ReadFromJsonAsync<AddressResponseDTO>();
       
       
       var client1= Factory.CreateClient();
       
         var payload1 = new
         {
             clientId = newClientResponse.Id,
             addressId = newAddressResponse!.Id,
             paymentMethod = 1,
             items = new [] { new { productId = newProductResponse!.Id, quantity = 10, description = "string" } }
         };

         var payload2 = new
         {
             clientId = 1,
             addressId = 1,
             paymentMethod = 1,
             items = new [] { new { productId = 1, quantity = 10, description = "string" } }
         };

         var task = await client1.PostAsJsonAsync("/Order", payload1);
        
         Assert.Equal(HttpStatusCode.OK,task.StatusCode);
    }
    
}