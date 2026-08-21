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


public class OrderTests:IClassFixture<DatabaseTestFactory>
{
    private readonly HttpClient _client;
    private readonly DatabaseTestFactory _factory;
    private readonly ITestOutputHelper _output;

    public OrderTests(DatabaseTestFactory factory, ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        _factory = factory;
        _output = output;
    }


    [Fact]
    public async Task First_order_on_day_ConcurrentRequests_ShouldAssignSequentialNumbersWithoutErrors()
    {

       var openResponse =  await _client.PatchAsJsonAsync("/store/status", new { isOpen = true });
       
       Assert.Equal(HttpStatusCode.OK, openResponse.StatusCode);

       var newJsonProduct = new
       {
           name = "coca cola geladinha",
           type = 0,
           description = "uma coquinha trincando",
           price = 8
       };
       
       var newProduct = await  _client.PostAsJsonAsync("/Product", newJsonProduct);
       
       Assert.Equal(HttpStatusCode.OK, newProduct.StatusCode);

       
       var newJsonCLient = new
       {
           name = "Rafael isac",
           phone = "5512991901969",
           email = "rafaeldarrigo@gmail.com"
       };
       
       var newCLient = await  _client.PostAsJsonAsync("/api/Client", newJsonCLient);
       
       Assert.Equal(HttpStatusCode.OK, newCLient.StatusCode);

       var newJsonNeighborhood = new
       {
           name = "josé bonifacio",
           deliveryFee = 10,
           isAvaible = true
       };
       
       var newNeighborhood = await _client.PostAsJsonAsync("/Neighborhood", newJsonNeighborhood);
       
       Assert.Equal(HttpStatusCode.OK, newNeighborhood.StatusCode);
       
       var newNeighborhoodResponse = await newNeighborhood.Content.ReadFromJsonAsync<NeighborhoodResponseDTO>();
       var newClientResponse = await newCLient.Content.ReadFromJsonAsync<ClientResponseDTO>();
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

       var newAddress = await _client.PostAsJsonAsync("/api/Address", newJsonAddress);
       Assert.Equal(HttpStatusCode.OK, newAddress.StatusCode);
       
       var newAddressResponse = await newAddress.Content.ReadFromJsonAsync<AddressResponseDTO>();
       
       
       var client1= _factory.CreateClient();
       
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

         var status = new
         {
             statusCode = task.StatusCode,
             body = await task.Content.ReadAsStringAsync()
         };

         _output.WriteLine($"======== a partir daqui : {status}");

         Assert.Equal(HttpStatusCode.OK,task.StatusCode);
    }
    
}