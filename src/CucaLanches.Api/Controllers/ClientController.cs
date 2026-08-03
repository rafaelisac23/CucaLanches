using CucaLanches.Application.Clients.DTOs;
using CucaLanches.Application.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ClientController:ControllerBase
{
   private readonly IClientService _clientService;

   public ClientController(IClientService  clientService)
   {
      _clientService = clientService;
   }

   [HttpPost("identify")]
   public async Task<ActionResult<ClientResponseDTO>> IdentifyClient([FromBody]IdentifyClientRequestDTO request)
   {
      var client = await _clientService.IdentifyCLient(request);

      return Ok(client);
   }

   [HttpPost]
   public async Task<ActionResult<ClientResponseDTO>> PostClient(ClientRequestDTO request)
   {
      var newClient = await _clientService.CreateClient(request);
      return Ok(newClient);
   }

   [HttpPatch("{id:int}")]
   public async Task<ActionResult<ClientResponseDTO>> PatchClient(int id,ClientUpdateRequestDTO request)
   {
      var updatedClient = await _clientService.UpdateClient(id,request);
      return Ok(updatedClient);
   }
}