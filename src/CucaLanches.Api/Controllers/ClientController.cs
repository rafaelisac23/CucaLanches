using System.Security.Claims;
using CucaLanches.Application.Clients.DTOs;
using CucaLanches.Application.Clients.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

   [HttpPatch]
   [Authorize(Policy = "ClientOnly")]
   public async Task<ActionResult<ClientResponseDTO>> PatchClient(ClientUpdateRequestDTO request)
   {
      
      var clientid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      
      var updatedClient = await _clientService.UpdateClient(clientid,request);
      return Ok(updatedClient);
   }
}