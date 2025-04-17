using CarRental.src.Models;

namespace WebApiTests.TestData;
public static class TestCarModels
{
    public static readonly CarModel Model3 = new(
        code: "TESLA_MODEL_3",
        name: "Tesla Model 3",
        description: "All-electric fastback",
        thumbnailUrl: "test-url"
    );

    public static readonly CarModel ModelS = new(
        code: "TESLA_MODEL_S",
        name: "Tesla Model S",
        description: "Luxury electric sedan",
        thumbnailUrl: "test-url"
    );

    public static readonly CarModel ModelX = new(
        code: "TESLA_MODEL_X",
        name: "Tesla Model X",
        description: "Premium electric SUV",
        thumbnailUrl: "test-url"
    );

    public static readonly CarModel ModelY = new(
        code: "TESLA_MODEL_Y",
        name: "Tesla Model Y",
        description: "Compact electric SUV",
        thumbnailUrl: "test-url"
    );
}