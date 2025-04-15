using CarRental.src.DTOs.Reservation;
using CarRental.src.Models;
using CarRental.src.Services.Interfaces;
using FluentResults;

sealed class ReservationService : IReservationService
{
    private readonly IPricingService _pricingService;
    private readonly ICarRepository _carRepository;

    public ReservationService(IPricingService pricingService, ICarRepository carRepository)
    {
        _pricingService = pricingService;
        _carRepository = carRepository;
    }

    public async Result<ReservationResponse> ReserveCar(ReservationRequest request)
    {
        // Check availability for the dates
        // Get all cars that match the request
        // Check which are free
        List<Car> cars = await _carRepository.GetById(request.CarId);
    }
}