using CarRental.src.DTOs.Car;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;
using Moq;
using CarRental.src.Services.Implementations;
using CarRental.src.Services.Interfaces;
using WebApiTests.TestData;

namespace WebApiTests.UnitTests.Services;

public class CarServiceTests
{
    private readonly Mock<ICarRepository> _carRepositoryMock;
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<ICarPricingRuleRepository> _carPricingRepositoryMock;
    private readonly CarService _carService;

    public CarServiceTests()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _carPricingRepositoryMock = new Mock<ICarPricingRuleRepository>();
        
        _carService = new CarService(
            _carRepositoryMock.Object,
            _reservationRepositoryMock.Object,
            _carPricingRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAvailableCars_WithInvalidDates_ReturnsValidationError()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(3);
        var endDate = DateTime.Now.AddDays(1);
        var carModel = "TESLA_MODEL_3";

        // Act
        var result = await _carService.GetAvailableCars(carModel, startDate, endDate);

        // Assert
        Assert.True(result.IsFailed);
        var error = Assert.IsType<ValidationError>(result.Errors[0]);
        Assert.Equal("startDate", error.PropertyName);
    }

    [Fact]
    public async Task GetAvailableCars_WithNoMatchingCars_ReturnsNotFoundError()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(3);
        var carModel = "TESLA_MODEL_3";

        _carRepositoryMock
            .Setup(x => x.GetByModel(carModel))
            .ReturnsAsync(new List<Car>());

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(carModel, startDate, endDate))
            .ReturnsAsync(new List<Reservation>());

        // Act
        var result = await _carService.GetAvailableCars(carModel, startDate, endDate);

        // Assert
        Assert.True(result.IsFailed);
        Assert.IsType<NotFoundError>(result.Errors[0]);
    }

    [Fact]
    public async Task GetAvailableCars_WithAvailableCars_ReturnsAvailableCars()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(3);
        var carModel = "TESLA_MODEL_3";
        var carId = Guid.NewGuid();

        var cars = new List<Car>
        {
            new() 
            { 
                Id = carId,
                RegistrationNumber = "1234TM3",
                CarModel = TestCarModels.Model3,
                CarModelId = TestCarModels.Model3.Id
            }
        };

        var pricing = new CarPricingRule
        {
            CarModel = TestCarModels.Model3,
            CarModelId = TestCarModels.Model3.Id,
            DailyRate = 100.0
        };

        _carRepositoryMock
            .Setup(x => x.GetByModel(carModel))
            .ReturnsAsync(cars);

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(carModel, startDate, endDate))
            .ReturnsAsync(new List<Reservation>());

        _carPricingRepositoryMock
            .Setup(x => x.GetByModel(TestCarModels.Model3))
            .ReturnsAsync(pricing);

        // Act
        var result = await _carService.GetAvailableCars(carModel, startDate, endDate);

        // Assert
        Assert.True(result.IsSuccess);
        var availableCars = result.Value;
        Assert.Single(availableCars);
        Assert.Equal(carId, availableCars[0].Id);
        Assert.Equal("1234TM3", availableCars[0].RegistrationNumber);
        Assert.Equal("TESLA_MODEL_3", availableCars[0].CarModel);
        Assert.Equal(200.0, availableCars[0].TotalPrice, precision: 2);
    }

    [Fact]
    public async Task GetAvailableCars_WithReservedCars_FiltersOutReservedCars()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(3);
        var carModel = "TESLA_MODEL_3";
        var availableCarId = Guid.NewGuid();
        var reservedCarId = Guid.NewGuid();

        var cars = new List<Car>
        {
            new() 
            { 
                Id = availableCarId,
                RegistrationNumber = "1234TM3",
                CarModel = TestCarModels.Model3,
                CarModelId = TestCarModels.Model3.Id
            },
            new() 
            { 
                Id = reservedCarId,
                RegistrationNumber = "5678TM3",
                CarModel = TestCarModels.Model3,
                CarModelId = TestCarModels.Model3.Id
            }
        };

        var reservations = new List<Reservation>
        {
            new(
                car: cars[1],
                customerName: "Test User",
                customerEmail: "test@example.com",
                startDate: startDate,
                endDate: endDate,
                totalCost: 200.0,
                pickupLocation: new Location("PAP", "Test Location", "Test Address"),
                returnLocation: new Location("PAP", "Test Location", "Test Address"),
                reservationStatus: ReservationStatus.Confirmed
            )
        };

        var pricing = new CarPricingRule
        {
            CarModel = TestCarModels.Model3,
            CarModelId = TestCarModels.Model3.Id,
            DailyRate = 100.0
        };

        _carRepositoryMock
            .Setup(x => x.GetByModel(carModel))
            .ReturnsAsync(cars);

        _reservationRepositoryMock
            .Setup(x => x.GetByModelAndDate(carModel, startDate, endDate))
            .ReturnsAsync(reservations);

        _carPricingRepositoryMock
            .Setup(x => x.GetByModel(TestCarModels.Model3))
            .ReturnsAsync(pricing);

        // Act
        var result = await _carService.GetAvailableCars(carModel, startDate, endDate);

        // Assert
        Assert.True(result.IsSuccess);
        var availableCars = result.Value;
        Assert.Single(availableCars);
        Assert.Equal(availableCarId, availableCars[0].Id);
        Assert.Equal("1234TM3", availableCars[0].RegistrationNumber);
    }
} 