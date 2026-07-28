using CucaLanches.Application.Neighborhoods.DTOs;
using CucaLanches.Application.Neighborhoods.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NeighborhoodController:ControllerBase
{
    
    private readonly INeighborhoodService _Service;

    public NeighborhoodController(INeighborhoodService service)
    {
        _Service = service;
    }

    [HttpGet]
    public async Task<List<NeighborhoodResponseDTO>> GetAllAsync([FromQuery] bool all)
    {
        var neighborhoods = await _Service.ListAsync(all);
        
        return  neighborhoods;
    }

    [HttpPost]
    public async Task<ActionResult<NeighborhoodResponseDTO>> Post(NeighborhoodRequestDTO neighborhoodRequest)
    {
        
        var neighborhood = await _Service.CreateAsync(neighborhoodRequest);
        
        return Ok(neighborhood);

    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch([FromRoute]int id,[FromBody]NeighborhoodUpdateRequestDTO request)
    {
        var updatedNeighborhood = await _Service.UpdateAsync(id, request);
        return Ok(updatedNeighborhood);
    }
    
    
}