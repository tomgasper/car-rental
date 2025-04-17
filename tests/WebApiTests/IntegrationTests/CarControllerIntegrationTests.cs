using System.Net;
using System.Net.Http.Json;
using CarRental.src.DTOs.Car;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace WebApiTests.IntegrationTests;

public class CarControllerIntegrationTests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CarControllerIntegrationTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetCars_WithValidRequest_ReturnsAvailableCars()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(60);
        var endDate = DateTime.Now.AddDays(62);
        var carModel = "TESLA_MODEL_3";

        // Act
        var response = await _client.GetAsync(
            $"/api/cars?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&carModel={carModel}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var cars = await response.Content.ReadFromJsonAsync<List<CarAvailabilityResponse>>();
        Assert.NotNull(cars);
        Assert.NotEmpty(cars);
        
        // Verify car properties based on seeded data
        var firstCar = cars[0];
        Assert.Equal("TESLA_MODEL_3", firstCar.CarModelCode);
        Assert.NotEqual(Guid.Empty, firstCar.Id);
        Assert.NotEmpty(firstCar.RegistrationNumber);
        Assert.True(firstCar.TotalPrice > 0);
    }

    [Fact]
    public async Task GetCars_WithInvalidDates_ReturnsBadRequest()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(43);
        var endDate = DateTime.Now.AddDays(41);
        var carModel = "TESLA_MODEL_3";

        // Act
        var response = await _client.GetAsync(
            $"/api/cars?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&carModel={carModel}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCars_WithNonExistentModel_ReturnsNotFound()
    {
        // Arrange
        var startDate = DateTime.Now.AddDays(61);
        var endDate = DateTime.Now.AddDays(63);
        var carModel = "NON_EXISTENT_MODEL";

        // Act
        var response = await _client.GetAsync(
            $"/api/cars?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&carModel={carModel}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
} 