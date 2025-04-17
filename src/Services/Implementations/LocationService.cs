using CarRental.DTOs.Location;
using CarRental.src.Repositories.Interfaces;
using CarRental.src.Services.Interfaces;
using FluentResults;

namespace CarRental.src.Services.Implementations;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<Result<List<LocationResponse>>> GetLocations()
    {
        var locations = await _locationRepository.GetAll();

        if (!locations.Any())
        {
            return Result.Fail<List<LocationResponse>>(new NotFoundError("No locations were found."));
        }

        var response = locations.Select(location => new LocationResponse(
            Id: location.LocationCode,
            Name: location.Name,
            Address: location.Address
        )).ToList();

        return Result.Ok(response);
    }
} 