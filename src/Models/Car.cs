namespace CarRental.src.Models;
public class Car
{
    public Guid Id { get;set; }
    public string RegistrationNumber { get; set; } = null!;
    public Guid CarModelId { get; set; }
    public CarModel CarModel { get; set; } = null!;
}