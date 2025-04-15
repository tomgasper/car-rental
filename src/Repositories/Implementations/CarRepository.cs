using CarRental.src.Infrastructure;
using CarRental.src.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.src.Repositories.Implementations;

class CarRepository : ICarRepository
{
    private readonly AppDbContext _dbContext;
    public async Task<List<Car>> GetByModel(CarModel carModel)
    {
        return await _dbContext.Cars.Where( car => car.CarModel == carModel).ToListAsync();
    }
}