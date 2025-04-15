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
    private readonly IUnitOfWork _unitOfWork;

    public ReservationService(IPricingService pricingService, ICarRepository carRepository, IReservationRepository reservationRepository, IUnitOfWork unitOfWork)
    {
        _pricingService = pricingService;
        _carRepository = carRepository;
        _reservationRepository = reservationRepository;
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

        // Make reservation and persist it
        var reservation = new Reservation(
            car: car,
            startDate: request.StartDate,
            endDate: request.EndDate,
            totalCost: totalCost,
            reservationStatus: ReservationStatus.Confirmed
        );
        _reservationRepository.AddReservation(reservation);

        await _unitOfWork.SaveChangesAsync();

        return new ReservationResponse(
            ReservationId: reservation.Id,
            CustomerName: request.FirstName + " " + request.LastName,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            CarId: request.CarId,
            TotalCost: totalCost,
            ReservationStatus: reservation.ReservationStatus.ToString()
        );
    }
}