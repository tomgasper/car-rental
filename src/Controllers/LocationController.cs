using CarRental.src.DTOs.Location;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[Route("v1/api/locations")]
public sealed class LocationController : ApiController
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLocations()
    {
        Result<List<LocationResponse>> result = await _locationService.GetLocations();

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }
}
