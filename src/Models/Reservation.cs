namespace CarRental.src.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = null!;
    public string CustomerEmail { get; set; } = null!;
    public string CustomerPhoneNumber { get; set; } = null!;
    public Guid CarId { get; set; }
    public Car Car { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double TotalCost { get; set; }
    public ReservationStatus ReservationStatus { get; set; }
    public Guid PickupLocationId { get; set; }
    public Location PickupLocation { get; set; } = null!;
    public Guid ReturnLocationId { get;set; }
    public Location ReturnLocation { get; set; } = null!;

    protected Reservation() { }

    public Reservation(
        Car car,
        string customerName,
        string customerEmail,
        string customerPhoneNumber,
        DateTime startDate,
        DateTime endDate,
        double totalCost,
        Location pickupLocation,
        Location returnLocation,
        ReservationStatus reservationStatus
    ) {
        Id = Guid.NewGuid();
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        CustomerPhoneNumber = customerPhoneNumber;
        Car = car;
        CarId = car.Id;
        StartDate = startDate;
        EndDate = endDate;
        TotalCost = totalCost;
        PickupLocation = pickupLocation;
        PickupLocationId = pickupLocation.Id;
        ReturnLocation = returnLocation;
        ReturnLocationId = returnLocation.Id;
        ReservationStatus = reservationStatus;
    }
}