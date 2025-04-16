using CarRental.src.Infrastructure;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.src.Repositories.Implementations;

public class CarPricingRepository : ICarPricingRuleRepository
{
    private readonly AppDbContext _dbContext;

    public CarPricingRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CarPricingRule?> GetByModel(CarModel carModel)
    {
        return await _dbContext.CarPricingRules.FirstOrDefaultAsync(pricing => pricing.CarModel == carModel);
    }
}