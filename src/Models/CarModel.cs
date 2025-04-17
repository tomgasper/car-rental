namespace CarRental.src.Models;

public class CarModel
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!; // f.e "TESLA_MODEL_S", "TESLA_MODEL_3", etc.
    public string Name { get; set; } = null!; // f.e "Tesla Model S", "Tesla Model 3"
    public string Description { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = null!;
    
    public ICollection<Car> Cars { get; set; } = new List<Car>();

    protected CarModel() { }

    public CarModel(
        string code,
        string name,
        string description,
        string thumbnailUrl)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Description = description;
        ThumbnailUrl = thumbnailUrl;
    }
}