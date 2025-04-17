using CarRental.src.Infrastructure;
using CarRental.src.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.src.Repositories.Implementations;

class CarRepository : ICarRepository
{
    private readonly AppDbContext _dbContext;

    public CarRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Car?> GetById(Guid carId)
    {
        return await _dbContext.Cars.FirstOrDefaultAsync( car => car.Id == carId);
    }

    public async Task<List<Car>> GetByModel(string carModelCode)
    {
        return await _dbContext.Cars.Include(c => c.CarModel).Where( model => model.CarModel.Code == carModelCode).ToListAsync();
    }

    public async Task<List<CarModel>> GetAllModels()
    {
        return await _dbContext.CarModels
            .AsNoTracking()
            .ToListAsync();
    }
}