using CarRental.src.DTOs.Car;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CarRental.src.Tests.Unit.Controllers;

public class CarControllerTests
{
    private readonly Mock<ICarService> _carServiceMock;
    private readonly CarController _controller;

    public CarControllerTests()
    {
        _carServiceMock = new Mock<ICarService>();
        _controller = new CarController(_carServiceMock.Object);
    }

    [Fact]
    public async Task GetCars_WithValidRequest_ReturnsAvailableCars()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(3);
        var carModel = "Model3";
        var expectedCars = new List<CarAvailabilityResponse>
        {
            new(
                Id: Guid.NewGuid(),
                RegistrationNumber: "1234TM3",
                CarModel: "Model3",
                TotalPrice: 300.0
            )
        };

        _carServiceMock
            .Setup(x => x.GetAvailableCars(carModel, startDate, endDate))
            .ReturnsAsync(Result.Ok(expectedCars));

        // Act
        var result = await _controller.GetCars(startDate, endDate, carModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);  // Instead of Assert.IsNotNull
        Assert.Equal(200, okResult.StatusCode);
        
        var returnedCars = Assert.IsType<List<CarAvailabilityResponse>>(okResult.Value);
        Assert.Equal(expectedCars.Count, returnedCars.Count);
    }

    [Fact]
    public async Task GetCars_WithInvalidDates_ReturnsBadRequest()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(3);
        var endDate = DateTime.Now.AddDays(1);
        var carModel = "Model3";

        _carServiceMock
            .Setup(x => x.GetAvailableCars(carModel, startDate, endDate))
            .ReturnsAsync(Result.Fail(new ValidationError("Start date must be before end date", "startDate")));

        // Act
        var result = await _controller.GetCars(startDate, endDate, carModel);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, problemResult.StatusCode);
    }

    [Fact]
    public async Task GetCars_WithNonExistentModel_ReturnsNotFound()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(3);
        var carModel = "NonExistentModel";

        _carServiceMock
            .Setup(x => x.GetAvailableCars(carModel, startDate, endDate))
            .ReturnsAsync(Result.Fail(new NotFoundError($"No cars were found for the provided dates and model: {carModel}.")));

        // Act
        var result = await _controller.GetCars(startDate, endDate, carModel);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, problemResult.StatusCode);
    }
}