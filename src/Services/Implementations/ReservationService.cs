using CarRental.src.DTOs.Reservation;
using CarRental.src.Infrastructure.UnitOfWork;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;
using CarRental.src.Services.Interfaces;
using FluentResults;

sealed class ReservationService : IReservationService
{
    private readonly IPricingService _pricingService;
    private readonly ICarRepository _carRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReservationService(IPricingService pricingService, ICarRepository carRepository, IReservationRepository reservationRepository, ILocationRepository locationRepository, IUnitOfWork unitOfWork)
    {
        _pricingService = pricingService;
        _carRepository = carRepository;
        _reservationRepository = reservationRepository;
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReservationResponse>> ReserveCar(ReservationRequest request)
    {
        Car? car = await _carRepository.GetById(request.CarId);
        if (car is null)
        {
            return Result.Fail<ReservationResponse>(new NotFoundError($"The car with id: {request.CarId} couldn't be found"));
        }

        // Calculate the total cost
        var totalCost = _pricingService.CalculatePrice(car.CarModel, request.StartDate, request.EndDate);
        if (totalCost.IsFailed) {
            return Result.Fail<ReservationResponse>(totalCost.Errors);
        }

        // Make reservation and persist it
        Location? pickupLocation = await _locationRepository.GetByCode(Location.NormalizeCode(request.PickupLocation));
        Location? returnLocation = await _locationRepository.GetByCode(Location.NormalizeCode(request.ReturnLocation));

        if (pickupLocation is null || returnLocation is null) {
            return Result.Fail<ReservationResponse>(new NotFoundError($"Pick up location with code: {pickupLocation} or return location with code: {returnLocation} couldn't be found."));
        }
        
        var reservation = new Reservation(
            car: car,
            customerName: request.FirstName.Trim() + " " + request.LastName.Trim(),
            customerEmail: request.Email,
            startDate: request.StartDate,
            endDate: request.EndDate,
            totalCost: totalCost.Value,
            pickupLocation: pickupLocation,
            returnLocation: returnLocation,
            reservationStatus: ReservationStatus.Confirmed
        );
        _reservationRepository.AddReservation(reservation);

        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        return new ReservationResponse(
            ReservationId: reservation.Id,
            CustomerName: reservation.CustomerName,
            CustomerEmail: reservation.CustomerEmail,
            StartDate: reservation.StartDate,
            EndDate: reservation.EndDate,
            CarId: reservation.Car.Id,
            TotalCost: totalCost.Value,
            PickupLocation: pickupLocation.Name,
            ReturnLocation: returnLocation.Name,
            ReservationStatus: reservation.ReservationStatus.ToString()
        );
    }
}