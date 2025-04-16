using CarRental.src.DTOs.Car;
using FluentResults;

namespace CarRental.src.Services.Interfaces;
interface ICarService
{
    Task<Result<List<CarAvailabilityResponse>>> GetAvailableCars(string? carModel, DateTime startDate, DateTime endDate);
} 