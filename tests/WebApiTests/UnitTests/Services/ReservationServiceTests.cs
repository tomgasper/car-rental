using CarRental.src.DTOs.Reservation;
using CarRental.src.Infrastructure.UnitOfWork;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;
using CarRental.src.Services.Implementations;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Moq;

namespace WebApiTests.UnitTests.Services;

public class ReservationServiceTests
{
    private readonly Mock<IPricingService> _pricingServiceMock;
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<ILocationRepository> _locationRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ReservationService _service;

    public ReservationServiceTests()
    {
        _pricingServiceMock = new Mock<IPricingService>();
        _carRepositoryMock = new Mock<ICarRepository>();
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _locationRepositoryMock = new Mock<ILocationRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _service = new ReservationService(
            _pricingServiceMock.Object,
            _carRepositoryMock.Object,
            _reservationRepositoryMock.Object,
            _locationRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task ReserveCar_WithValidRequest_ReturnsSuccessfulReservation()
    {
        // Arrange
        var request = new ReservationRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(3),
            CarModel: "Model3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = CarModel.Model3
        };

        var pickupLocation = new Location("PAP", "Palma Airport", "Airport address");
        var returnLocation = new Location("PAP", "Palma Airport", "Airport address");

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation>());

        _pricingServiceMock
            .Setup(x => x.CalculatePrice(CarModel.Model3, request.StartDate, request.EndDate))
            .Returns(Result.Ok(300.0));

        _locationRepositoryMock
            .Setup(x => x.GetByCode("PAP"))
            .ReturnsAsync(pickupLocation);

        // Act
        var result = await _service.ReserveCar(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("John Doe", result.Value.CustomerName);
        Assert.Equal(car.Id, result.Value.CarId);
        Assert.Equal(300.0, result.Value.TotalCost);
        Assert.Equal("Palma Airport", result.Value.PickupLocation);
        Assert.Equal("Confirmed", result.Value.ReservationStatus);

        _reservationRepositoryMock.Verify(x => x.AddReservation(It.IsAny<Reservation>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReserveCar_WithNoAvailableCars_ReturnsFailure()
    {
        // Arrange
        var request = new ReservationRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(3),
            CarModel: "Model3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = CarModel.Model3
        };

        var existingReservation = new Reservation(
            car: car,
            customerName: "Existing Customer",
            customerEmail: "existing@example.com",
            startDate: request.StartDate,
            endDate: request.EndDate,
            totalCost: 300.0,
            pickupLocation: new Location("PAP", "Palma Airport", "Address"),
            returnLocation: new Location("PAP", "Palma Airport", "Address"),
            reservationStatus: ReservationStatus.Confirmed
        );

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation> { existingReservation });

        // Act
        var result = await _service.ReserveCar(request);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, error => error.Message.Contains($"No {request.CarModel} was found"));
    }

    [Fact]
    public async Task ReserveCar_WithInvalidLocation_ReturnsFailure()
    {
        // Arrange
        var request = new ReservationRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(3),
            CarModel: "Model3",
            PickupLocation: "INVALID",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = CarModel.Model3
        };

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation>());

        _pricingServiceMock
            .Setup(x => x.CalculatePrice(CarModel.Model3, request.StartDate, request.EndDate))
            .Returns(Result.Ok(300.0));

        // Setup both location calls
        _locationRepositoryMock
            .Setup(x => x.GetByCode("INVALID"))
            .ReturnsAsync((Location?)null);

        _locationRepositoryMock
            .Setup(x => x.GetByCode("PAP"))
            .ReturnsAsync(new Location("PAP", "Palma Airport", "Airport address"));

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.ReserveCar(request));
    }

    [Fact]
    public async Task ReserveCar_WithPricingError_ReturnsFailure()
    {
        // Arrange
        var request = new ReservationRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(3),
            CarModel: "Model3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = CarModel.Model3
        };

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation>());

        _pricingServiceMock
            .Setup(x => x.CalculatePrice(CarModel.Model3, request.StartDate, request.EndDate))
            .Returns(Result.Fail<double>("Pricing calculation failed"));

        // Act
        var result = await _service.ReserveCar(request);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, error => error.Message.Contains("Pricing calculation failed"));
    }

    [Fact]
    public async Task ReserveCar_WithSameDayReservation_ReturnsOk()
    {
        // Arrange
        var request = new ReservationRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(1),
            CarModel: "Model3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var pickupReturnLocation = new Location("PAP", "Palma Airport", "Airport address");

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = CarModel.Model3
        };

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation>());

        _pricingServiceMock
        .Setup(x => x.CalculatePrice(CarModel.Model3, request.StartDate, request.EndDate))
        .Returns(Result.Ok(100.0));

        _locationRepositoryMock
            .Setup(x => x.GetByCode("PAP"))
            .ReturnsAsync(pickupReturnLocation);

        // Act
        var result = await _service.ReserveCar(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("John Doe", result.Value.CustomerName);
        Assert.Equal(car.Id, result.Value.CarId);
        Assert.Equal(100.0, result.Value.TotalCost);
        Assert.Equal("Palma Airport", result.Value.PickupLocation);
        Assert.Equal("Confirmed", result.Value.ReservationStatus);

        _reservationRepositoryMock.Verify(x => x.AddReservation(It.IsAny<Reservation>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReserveCar_WithOverlappingReturnDate_ReturnsFailure()
    {
        // Arrange
        var existingStartDate = DateTime.Now.AddDays(1);
        var existingEndDate = DateTime.Now.AddDays(3);
        var newStartDate = existingEndDate;
        var newEndDate = DateTime.Now.AddDays(5);

        var pickupReturnLocation = new Location("PAP", "Palma Airport", "Airport address");

        var request = new ReservationRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            StartDate: newStartDate,
            EndDate: newEndDate,
            CarModel: "Model3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = CarModel.Model3
        };

        var existingReservation = new Reservation(
            car: car,
            customerName: "Existing Customer",
            customerEmail: "existing@example.com",
            startDate: existingStartDate,
            endDate: existingEndDate,
            totalCost: 300.0,
            pickupLocation: pickupReturnLocation,
            returnLocation: pickupReturnLocation,
            reservationStatus: ReservationStatus.Confirmed
        );

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation> { existingReservation });
        
        _locationRepositoryMock
            .Setup(x => x.GetByCode("PAP"))
            .ReturnsAsync(pickupReturnLocation);

        // Act
        var result = await _service.ReserveCar(request);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, error => error.Message.Contains("No Model3 was found for the provided dates."));
    }
} 