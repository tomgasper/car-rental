namespace CarRental.src.DTOs.Reservation;
public record ReservationResponseDTO(
    Guid ReservationId,
    string CustomerName,
    DateTime StartDate,
    DateTime EndDate,
    Guid CarId,
    decimal TotalCost,
    string ReservationStatus);