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

    public async Task<List<Car>> GetByModel(string carModel)
    {
        if (Enum.TryParse<CarModel>(carModel, out CarModel parsedModel))
        {
            return await _dbContext.Cars.Where(car => car.CarModel == parsedModel).ToListAsync();
        }
        
        return new List<Car>();
    }
}