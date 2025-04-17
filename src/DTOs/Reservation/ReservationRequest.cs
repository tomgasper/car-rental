namespace CarRental.src.DTOs.Reservation;
public record ReservationRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateTime StartDate,
    DateTime EndDate,
    string CarModel,
    string PickupLocation,
    string ReturnLocation
);