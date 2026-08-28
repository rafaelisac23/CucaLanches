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
    // 1. Preparação dos dados necessários
    var openResponse = await Client.PatchAsJsonAsync("/store/status", new { isOpen = true });
    Assert.Equal(HttpStatusCode.OK, openResponse.StatusCode);

    var newJsonProduct = new
    {
        name = "coca cola geladinha",
        type = 0,
        description = "uma coquinha trincando",
        price = 8
    };
    var newProduct = await Client.PostAsJsonAsync("/Product", newJsonProduct);
    Assert.Equal(HttpStatusCode.OK, newProduct.StatusCode);

    var newJsonClient = new
    {
        name = "Rafael Isac",
        phone = "5512991901969",
        email = "rafaeldarrigo@gmail.com"
    };
    var newClient = await Client.PostAsJsonAsync("/api/Client", newJsonClient);
    Assert.Equal(HttpStatusCode.OK, newClient.StatusCode);

    var newJsonNeighborhood = new
    {
        name = "José Bonifácio",
        deliveryFee = 10,
        isAvaible = true
    };
    var newNeighborhood = await Client.PostAsJsonAsync("/Neighborhood", newJsonNeighborhood);
    Assert.Equal(HttpStatusCode.OK, newNeighborhood.StatusCode);

    var newNeighborhoodResponse = await newNeighborhood.Content.ReadFromJsonAsync<NeighborhoodResponseDTO>();
    var newClientResponse = await newClient.Content.ReadFromJsonAsync<ClientResponseDTO>();
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

    // 2. Monta 2 payloads com os IDs válidos criados
    var payload1 = new
    {
        clientId = newClientResponse.Id,
        addressId = newAddressResponse!.Id,
        paymentMethod = 1,
        items = new[] { new { productId = newProductResponse!.Id, quantity = 2, description = "Sem gelo" } }
    };

    var payload2 = new
    {
        clientId = newClientResponse.Id,
        addressId = newAddressResponse.Id,
        paymentMethod = 1,
        items = new[] { new { productId = newProductResponse.Id, quantity = 1, description = "Com gelo" } }
    };

    // 3. Cria dois HttpClients para simular dois usuários distintos acessando a API ao mesmo tempo
    var client1 = Factory.CreateClient();
    var client2 = Factory.CreateClient();

    // Dispara as duas requisições SIMULTANEAMENTE
    var task1 = client1.PostAsJsonAsync("/Order", payload1);
    var task2 = client2.PostAsJsonAsync("/Order", payload2);

    // Aguarda o término de ambas em paralelo
    var responses = await Task.WhenAll(task1, task2);

    // 4. Validações (Asserts)
    Assert.Equal(HttpStatusCode.OK, responses[0].StatusCode);
    Assert.Equal(HttpStatusCode.OK, responses[1].StatusCode);

    // (Opcional) Se o seu DTO de Order retornar o número do pedido (ex: OrderNumber ou DailyNumber):
    // var order1 = await responses[0].Content.ReadFromJsonAsync<OrderResponseDTO>();
    // var order2 = await responses[1].Content.ReadFromJsonAsync<OrderResponseDTO>();
    // Assert.NotEqual(order1!.DailyNumber, order2!.DailyNumber);
}
    
}