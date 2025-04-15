namespace CarRental.src.DTOs.Reservation;
record ReservationRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateTime BirthDate,
    DateTime StartDate,
    DateTime EndDate,
    Guid CarId,
    Guid PickupLocationId,
    Guid ReturnLocationId
);