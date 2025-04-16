using CarRental.src.Models;
using FluentResults;

namespace CarRental.src.Services.Interfaces;
public interface IPricingService
{
    Result<double> CalculatePrice(CarModel carModel, DateTime startDate, DateTime endDate);
}