using CucaLanches.Application.Addresses.DTOs;
using CucaLanches.Application.Addresses.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AddressController:ControllerBase
{
    
    public readonly IAddressService AddressService;

    public AddressController(IAddressService  addressService)
    {
        AddressService = addressService;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<List<AddressSumaryDTO>>> GetAll()
    {
        var result = await AddressService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AddressSumaryDTO>> GetByIdAsync(int id)
    {
        var result = await AddressService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AddressResponseDTO>> AddAsync(AddressRequestDTO requestDto)
    {
        var result = await AddressService.AddAsync(requestDto);
        return Ok(result);
    }

   [HttpPatch("{id}")]
    public async Task<ActionResult<AddressSumaryDTO>> PatchAsync(int id, AddressRequestPatchDTO requestDto)
    {
     var result = await AddressService.PatchAsync(id, requestDto);
     
     return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AddressSumaryDTO>> DeleteAsync(int id)
    {
        var result = await AddressService.RemoveAsync(id);

        return Ok(result);
    }
   
    
    
}