namespace CarRental.src.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public Car Car { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double TotalCost { get; set; }
    public ReservationStatus ReservationStatus { get; set; }

    public Reservation(
        Car car,
        DateTime startDate,
        DateTime endDate,
        double totalCost,
        ReservationStatus reservationStatus   
    ) {
        Id = new Guid();
        Car = car;
        StartDate = startDate;
        EndDate = endDate;
        TotalCost = totalCost;
        ReservationStatus = reservationStatus;
    }
}