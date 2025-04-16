using CarRental.src.DTOs.Car;
using FluentResults;

namespace CarRental.src.Services.Interfaces;
public interface ICarService
{
    Task<Result<List<CarAvailabilityResponse>>> GetAvailableCars(string carModel, DateTime startDate, DateTime endDate);
} 