using System.ComponentModel.DataAnnotations;
using CarRental.src.DTOs.Car;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[Route("v1/api/cars")]
public sealed class CarController : ApiController 
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCarModels()
    {
        Result<List<CarModelResponse>> result = await _carService.GetCarModels();   

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpGet("availability")]
    public async Task<IActionResult> GetAvailableCars(
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