namespace CarRental.src.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public string CustomerName { get;set; }
    public string CustomerEmail { get;set; }
    public Car Car { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double TotalCost { get; set; }
    public ReservationStatus ReservationStatus { get; set; }
    public Location PickupLocation { get; set; }
    public Location ReturnLocation { get;set; }

    public Reservation(
        Car car,
        string customerName,
        string customerEmail,
        DateTime startDate,
        DateTime endDate,
        double totalCost,
        Location pickupLocation,
        Location returnLocation,
        ReservationStatus reservationStatus
    ) {
        Id = new Guid();
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        Car = car;
        StartDate = startDate;
        EndDate = endDate;
        TotalCost = totalCost;
        PickupLocation = pickupLocation;
        ReturnLocation = returnLocation;
        ReservationStatus = reservationStatus;
    }
}