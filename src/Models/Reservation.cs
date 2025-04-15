namespace CarRental.src.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public Car Car { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double TotalCost { get; set; }
    public ReservationStatus ReservationStatus { get; set; }
}