using CarRental.src.DTOs.Reservation;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace WebApiTests.UnitTests.Controllers;

public class ReservationControllerTests
{
    private readonly Mock<IReservationService> _reservationServiceMock;
    private readonly ReservationController _controller;

    public ReservationControllerTests()
    {
        _reservationServiceMock = new Mock<IReservationService>();
        _controller = new ReservationController(_reservationServiceMock.Object);
    }

    [Fact]
    public async Task MakeReservation_WithValidRequest_ReturnsReservation()
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

        var expectedResponse = new ReservationResponse(
            ReservationId: Guid.NewGuid(),
            CustomerName: "John Doe",
            CustomerEmail: "john.doe@example.com",
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            CarId: Guid.NewGuid(),
            TotalCost: 300.0,
            PickupLocation: "Palma Airport",
            ReturnLocation: "Palma Airport",
            ReservationStatus: "Confirmed"
        );

        _reservationServiceMock
            .Setup(x => x.ReserveCar(request))
            .ReturnsAsync(Result.Ok(expectedResponse));

        // Act
        var result = await _controller.MakeReservation(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        
        var reservation = Assert.IsType<ReservationResponse>(okResult.Value);
        Assert.Equal(expectedResponse.ReservationId, reservation.ReservationId);
    }

    [Fact]
    public async Task MakeReservation_WithNoAvailableCars_ReturnsNotFound()
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

        _reservationServiceMock
            .Setup(x => x.ReserveCar(request))
            .ReturnsAsync(Result.Fail(new NotFoundError($"No {request.CarModel} was found for the provided dates.")));

        // Act
        var result = await _controller.MakeReservation(request);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, problemResult.StatusCode);
    }

    [Fact]
    public async Task MakeReservation_WithInvalidLocation_ReturnsBadRequest()
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

        _reservationServiceMock
            .Setup(x => x.ReserveCar(request))
            .ReturnsAsync(Result.Fail(new ValidationError("Invalid location code: INVALID", "pickupLocation")));

        // Act
        var result = await _controller.MakeReservation(request);

        // Assert
        var problemResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(400, problemResult.StatusCode);
    }
}