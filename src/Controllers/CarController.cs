using System.ComponentModel.DataAnnotations;
using CarRental.src.DTOs.Car;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[Route("api/cars")]
public sealed class CarController : ApiController 
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCars(
        [FromQuery][Required] DateTime startDate,
        [FromQuery][Required] DateTime endDate,
        [FromQuery][Required] string carModel)
    {
        Result<List<CarAvailabilityResponse>> result = await _carService.GetAvailableCars(carModel, startDate, endDate);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }
}