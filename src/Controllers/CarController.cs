using CarRental.src.DTOs.Car;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[Route("api/cars")]
sealed class CarController : ApiController 
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet("availability")]
    public async Task<IActionResult> GetCars(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string? carModel = null)
    {
        Result<List<CarAvailabilityResponse>> result = await _carService.GetAvailableCars(carModel, startDate, endDate);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }
}