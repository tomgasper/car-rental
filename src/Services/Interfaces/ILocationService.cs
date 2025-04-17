using CarRental.DTOs.Location;
using FluentResults;

namespace CarRental.src.Services.Interfaces;

public interface ILocationService
{
    Task<Result<List<LocationResponse>>> GetLocations();
} 