using CarRental.src.DTOs.Car;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace CarRental.src.Services.Implementations;

sealed class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly ICarPricingRuleRepository _carPricingRepository;

    public CarService(
        ICarRepository carRepository, 
        IReservationRepository reservationRepository,
        ICarPricingRuleRepository carPricingRepository)
    {
        _carRepository = carRepository;
        _reservationRepository = reservationRepository;
        _carPricingRepository = carPricingRepository;
    }

    public async Task<Result<List<CarAvailabilityResponse>>> GetAvailableCars(string carModel, DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return Result.Fail<List<CarAvailabilityResponse>>(new ValidationError("Start date must be before end date", "startDate"));
        }

        // Get all cars of the specified model
        List<Car> cars = await _carRepository.GetByModel(carModel ?? "");

        // Get existing reservations for the date range
        List<Reservation> reservations = await _reservationRepository.GetByModelAndDate(carModel ?? "", startDate, endDate);
        
        // Filter out reserved cars
        HashSet<Guid> reservedCars = reservations
            .Select(reservation => reservation.Car.Id)
            .ToHashSet();

        var availableCars = cars
            .Where(car => !reservedCars.Contains(car.Id))
            .ToList();

        if (!availableCars.Any())
        {
            return Result.Fail<List<CarAvailabilityResponse>>(new NotFoundError($"No cars were found for the provided dates and model: {carModel}."));
        }

        // Get pricing for available cars
        var carResponses = new List<CarAvailabilityResponse>();
        foreach (var car in availableCars)
        {
            var pricing = await _carPricingRepository.GetByModel(car.CarModel);
            if (pricing == null) continue;

            carResponses.Add(new CarAvailabilityResponse(
                Id: car.Id,
                RegistrationNumber: car.RegistrationNumber,
                CarModel: car.CarModel.ToString(),
                TotalPrice: pricing.DailyRate * (endDate - startDate).TotalDays
            ));
        }

        return Result.Ok(carResponses);
    }
} 