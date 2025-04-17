using CarRental.src.DTOs.Reservation;
using CarRental.src.Infrastructure.UnitOfWork;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;
using CarRental.src.Services.Interfaces;
using FluentResults;
using FluentValidation;

public sealed class ReservationService : IReservationService
{
    private readonly IPricingService _pricingService;
    private readonly ICarRepository _carRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ReservationRequest> _validator;

    public ReservationService(
        IPricingService pricingService,
        ICarRepository carRepository,
        IReservationRepository reservationRepository,
        ILocationRepository locationRepository,
        IUnitOfWork unitOfWork,
        IValidator<ReservationRequest> validator)
    {
        _pricingService = pricingService;
        _carRepository = carRepository;
        _reservationRepository = reservationRepository;
        _locationRepository = locationRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<ReservationResponse>> ReserveCar(ReservationRequest request)
    {
        // Validate request
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result.Fail<ReservationResponse>(
                validationResult.Errors.Select(e => new ValidationError(e.ErrorMessage, e.PropertyName))
            );
        }

        var availableCar = await GetFirstAvailableCar(request.CarModel, request.StartDate, request.EndDate);

        if (availableCar.IsFailed){
            return Result.Fail<ReservationResponse>(availableCar.Errors);
        }

        // Calculate the total cost
        var totalCost = _pricingService.CalculatePrice(availableCar.Value.CarModel, request.StartDate, request.EndDate);
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
            car: availableCar.Value,
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

    private async Task<Result<Car>> GetFirstAvailableCar(string carModel, DateTime startDate, DateTime endDate)
    {
        // Filter out reserved cars
        List<Car> cars = await _carRepository.GetByModel(carModel);
        List<Reservation> reservations = await _reservationRepository.GetByModelAndDate(carModel, startDate, endDate);
        HashSet<Guid> reservedCars = reservations.Select( reservation => reservation.Car.Id ).ToHashSet();

        Car? availableCar = cars.FirstOrDefault( car => !reservedCars.Contains(car.Id));

        if (availableCar is null)
        {
            return Result.Fail<Car>(new NotFoundError($"No car was found for the provided dates."));
        }

        return availableCar;
    }
}