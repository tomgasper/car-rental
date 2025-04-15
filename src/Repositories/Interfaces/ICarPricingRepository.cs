using CarRental.src.Models;

interface ICarPricingRuleRepository {
    Task<CarPricingRule?> GetByModel(CarModel carModel);
}