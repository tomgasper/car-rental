namespace CarRental.src.DTOs.Car;

public record CarAvailabilityResponse(
    Guid Id,
    string RegistrationNumber,
    string CarModel,
    double TotalPrice
); 