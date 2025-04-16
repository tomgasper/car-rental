namespace CarRental.src.DTOs.Reservation;
public record ReservationRequest(
    string FirstName,
    string LastName,
    string Email,
    DateTime StartDate,
    DateTime EndDate,
    string CarModel,
    string PickupLocation,
    string ReturnLocation
);