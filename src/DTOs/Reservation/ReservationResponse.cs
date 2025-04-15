namespace CarRental.src.DTOs.Reservation;
public record ReservationResponse(
    Guid ReservationId,
    string CustomerName,
    DateTime StartDate,
    DateTime EndDate,
    Guid CarId,
    double TotalCost,
    string ReservationStatus);