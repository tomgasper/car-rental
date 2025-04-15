namespace CarRental.src.DTOs.Reservation;
public record ReservationResponse(
    Guid ReservationId,
    string CustomerName,
    string CustomerEmail,
    DateTime StartDate,
    DateTime EndDate,
    Guid CarId,
    double TotalCost,
    string PickupLocation,
    string ReturnLocation,
    string ReservationStatus
    );