using CarRental.src.Infrastructure;
using CarRental.src.Models;
using Microsoft.EntityFrameworkCore;

class LocationRepository : ILocationRepository
{
    private readonly AppDbContext _dbContext;

    public LocationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Location?> GetByCode(string locationCode)
    {
        return await _dbContext.Locations.FirstOrDefaultAsync(location => location.LocationCode.Equals(locationCode));
    }

    public async Task<List<Location>> GetAll()
    {
        return await _dbContext.Locations
            .AsNoTracking()
            .ToListAsync();
    }
}