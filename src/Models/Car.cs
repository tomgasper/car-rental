namespace CarRental.src.Models;
public class Car
{
    string Id { get;set; }
    string RegistrationNumber { get; set; }
    CarModel CarModel { get;set; }
    double Price { get;set; }
    Location Location { get; set; }
}