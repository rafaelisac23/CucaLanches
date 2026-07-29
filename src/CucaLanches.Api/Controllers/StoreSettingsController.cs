using CucaLanches.Application.StoreSettings.DTOs;
using CucaLanches.Application.StoreSettings.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CucaLanches.Api.Controllers;

[ApiController]
[Route("store")]
public class StoreSettingsController:ControllerBase
{
    
    private readonly IStoreSettingService _service;

    public StoreSettingsController(IStoreSettingService  service)
    {
        _service = service;
    }

    [HttpGet("status")]
    public async Task<ActionResult<StoreSettingsResponseDTO>> GetStatus()
    {
        var storeSetting = await _service.Get();
        return Ok(storeSetting);
    }
    
    [HttpPatch("status")]
    public async Task<ActionResult<StoreSettingsResponseDTO>> PatchStatus(StoreSettingsRequestDTO request)
    {
        var storeSetting = await _service.Patch(request);
        return Ok(storeSetting);
    }
    
}