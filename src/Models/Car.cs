namespace CarRental.src.Models;
public class Car
{
    public Guid Id { get;set; }
    public string RegistrationNumber { get; set; }
    public CarModel CarModel { get;set; }
    public Guid LocationId { get; set; }
    public Location Location { get; set; }
}