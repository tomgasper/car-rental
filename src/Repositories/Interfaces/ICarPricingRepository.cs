using CarRental.src.Models;

namespace CarRental.src.Repositories.Interfaces;
public interface ICarPricingRuleRepository
{
    Task<CarPricingRule?> GetByModel(CarModel carModel);
}