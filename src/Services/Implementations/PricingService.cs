using CarRental.src.Infrastructure;
using CarRental.src.Models;
using CarRental.src.Services.Interfaces;
using FluentResults;

sealed class PricingService : IPricingService
{
    private readonly AppDbContext _dbContext;

    public PricingService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Result<double> CalculatePrice(CarModel carModel, DateTime startDate, DateTime endDate)
    {
        if (startDate.Date > endDate.Date)
            return Result.Fail<double>(new ForbiddenError("Start date must be lower than the end date."));

        // Min rental range is one day
        int rentalDays = (endDate.Date - startDate.Date).Days;
        rentalDays = rentalDays == 0 ? 1 : rentalDays;

        var pricingRule = _dbContext.CarPricingRules
            .FirstOrDefault(r => r.CarModel == carModel);
        
        if (pricingRule == null)
            return Result.Fail<double>(new ForbiddenError($"Incorrect car model: {carModel}"));

        double totalPrice = pricingRule.DailyRate * rentalDays;
        return totalPrice;
    }
}