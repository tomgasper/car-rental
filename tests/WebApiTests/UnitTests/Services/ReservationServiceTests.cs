using CarRental.src.DTOs.Reservation;
using CarRental.src.Infrastructure.UnitOfWork;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;
using CarRental.src.Services.Implementations;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Moq;
using WebApiTests.TestData;
using FluentValidation;
namespace WebApiTests.UnitTests.Services;

public class ReservationServiceTests
{
    private readonly Mock<IPricingService> _pricingServiceMock;
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<ILocationRepository> _locationRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<ReservationRequest>> _validatorMock;
    private readonly ReservationService _service;

    public ReservationServiceTests()
    {
        _pricingServiceMock = new Mock<IPricingService>();
        _carRepositoryMock = new Mock<ICarRepository>();
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _locationRepositoryMock = new Mock<ILocationRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<ReservationRequest>>();

        _service = new ReservationService(
            _pricingServiceMock.Object,
            _carRepositoryMock.Object,
            _reservationRepositoryMock.Object,
            _locationRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object
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
            PhoneNumber: "+1234567890",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(3),
            CarModel: "TESLA_MODEL_3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = TestCarModels.Model3,
            CarModelId = TestCarModels.Model3.Id
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
            .Setup(x => x.CalculatePrice(TestCarModels.Model3, request.StartDate, request.EndDate))
            .Returns(Result.Ok(300.0));

        _locationRepositoryMock
            .Setup(x => x.GetByCode("PAP"))
            .ReturnsAsync(pickupLocation);

        // Add validator setup
        _validatorMock
            .Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

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
            PhoneNumber: "+1234567890",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(3),
            CarModel: "TESLA_MODEL_3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = TestCarModels.Model3,
            CarModelId = TestCarModels.Model3.Id
        };

        var existingReservation = new Reservation(
            car: car,
            customerName: "Existing Customer",
            customerEmail: "existing@example.com",
            customerPhoneNumber: "1234567890",
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

        _validatorMock
            .Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _service.ReserveCar(request);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, error => error.Message.Contains($"No car was found for the provided dates."));
    }

    [Fact]
    public async Task ReserveCar_WithInvalidLocation_ReturnsFailure()
    {
        // Arrange
        var request = new ReservationRequest(
            FirstName: "John",
            LastName: "Doe",
            Email: "john.doe@example.com",
            PhoneNumber: "+1234567890",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(3),
            CarModel: "TESLA_MODEL_3",
            PickupLocation: "INVALID",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = TestCarModels.Model3,
            CarModelId = TestCarModels.Model3.Id
        };

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation>());

        _pricingServiceMock
            .Setup(x => x.CalculatePrice(TestCarModels.Model3, request.StartDate, request.EndDate))
            .Returns(Result.Ok(300.0));

        _validatorMock
            .Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Setup both location calls
        _locationRepositoryMock
            .Setup(x => x.GetByCode("INVALID"))
            .ReturnsAsync((Location?)null);

        _locationRepositoryMock
            .Setup(x => x.GetByCode("PAP"))
            .ReturnsAsync(new Location("PAP", "Palma Airport", "Airport address"));

        // Act
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
            PhoneNumber: "+1234567890",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(3),
            CarModel: "TESLA_MODEL_3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = TestCarModels.Model3,
            CarModelId = TestCarModels.Model3.Id
        };

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation>());

        _pricingServiceMock
            .Setup(x => x.CalculatePrice(TestCarModels.Model3, request.StartDate, request.EndDate))
            .Returns(Result.Fail<double>("Pricing calculation failed"));

        _validatorMock
            .Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

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
            PhoneNumber: "+1234567890",
            StartDate: DateTime.Now.AddDays(1),
            EndDate: DateTime.Now.AddDays(1),
            CarModel: "TESLA_MODEL_3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var pickupReturnLocation = new Location("PAP", "Palma Airport", "Airport address");

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = TestCarModels.Model3,
            CarModelId = TestCarModels.Model3.Id
        };

        _carRepositoryMock
            .Setup(x => x.GetByModel(request.CarModel))
            .ReturnsAsync(new List<Car> { car });

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(request.CarModel, request.StartDate, request.EndDate))
            .ReturnsAsync(new List<Reservation>());

        _pricingServiceMock
            .Setup(x => x.CalculatePrice(TestCarModels.Model3, request.StartDate, request.EndDate))
            .Returns(Result.Ok(100.0));

        _locationRepositoryMock
            .Setup(x => x.GetByCode("PAP"))
            .ReturnsAsync(pickupReturnLocation);

        _validatorMock
            .Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

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
            PhoneNumber: "+1234567890",
            StartDate: newStartDate,
            EndDate: newEndDate,
            CarModel: "TESLA_MODEL_3",
            PickupLocation: "PAP",
            ReturnLocation: "PAP"
        );

        var car = new Car
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = "1234TM3",
            CarModel = TestCarModels.Model3,
            CarModelId = TestCarModels.Model3.Id
        };

        var existingReservation = new Reservation(
            car: car,
            customerName: "Existing Customer",
            customerEmail: "existing@example.com",
            customerPhoneNumber: "1234567890",
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

        _validatorMock
            .Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        // Act
        var result = await _service.ReserveCar(request);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, error => error.Message.Contains("No car was found for the provided dates."));
    }
} 