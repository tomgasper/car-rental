namespace CarRental.src.Models;

public class CarPricingRule
{
    public Guid Id { get; set; }
    public CarModel CarModel { get; set; }
    public double DailyRate { get; set; }
    public string Currency { get; set; } = "EUR";
}