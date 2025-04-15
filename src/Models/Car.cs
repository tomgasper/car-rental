namespace CarRental.src.Models;
public class Car
{
    public string Id { get;set; }
    public string RegistrationNumber { get; set; }
    public CarModel CarModel { get;set; }
    public double Price { get;set; }
    public Location Location { get; set; }
}