using CarRental.src.Models;

namespace CarRental.src.Services.Interfaces;
interface IPricingService
{
    double CalculatePrice(CarModel carModel, DateTime startDate, DateTime endDate);
}