using CarRental.src.Models;

namespace CarRental.src.Infrastructure.DataSeeding;

public static class InitialData
{    
    public static readonly List<Location> Locations = new()
    {
        new Location("PAP", "Palma Airport", "Airport Terminal 1, 07611 Palma, Illes Balears"),
        new Location("PCC", "Palma City Center", "Avinguda de Gabriel Roca, 07014 Palma, Illes Balears"),
        new Location("ALC", "Alcudia", "Carrer del Marisc, 07400 Alcúdia, Illes Balears"),
        new Location("MAN", "Manacor", "Avinguda del Parc, 07500 Manacor, Illes Balears")
    };

    public static readonly List<CarModel> CarModels = new()
    {
        new CarModel(
            code: "ModelS",
            name: "Tesla Model S",
            description: "Luxury electric sedan with exceptional range and performance",
            thumbnailUrl: "https://media.ed.edmunds-media.com/tesla/model-s/2025/oem/2025_tesla_model-s_sedan_plaid_fq_oem_1_1600.jpg"
        ),
        new CarModel(
            code: "Model3",
            name: "Tesla Model 3",
            description: "All-electric fastback that combines efficiency with style",
            thumbnailUrl: "https://www.milivolt.pl/wp-content/uploads/2020/02/Tesla_Model_3-102x.jpg"
        ),
        new CarModel(
            code: "ModelX",
            name: "Tesla Model X",
            description: "Premium electric SUV with falcon-wing doors",
            thumbnailUrl: "https://e-mobilni.pl/wp-content/uploads/2024/08/tesla-model-x-11.jpg"
        ),
        new CarModel(
            code: "ModelY",
            name: "Tesla Model Y",
            description: "Compact electric SUV with versatile interior space",
            thumbnailUrl: "https://cms.vehistools.pl/images/article/ng3JuiQYlL1E1JGTj0nbWWE1MLtTW0PY9zmnuD47.webp"
        )
    };

    public static readonly List<CarPricingRule> PricingRules = new()
    {
        new CarPricingRule { Id = Guid.NewGuid(), CarModelId = CarModels[0].Id, DailyRate = 150.00 },
        new CarPricingRule { Id = Guid.NewGuid(), CarModelId = CarModels[1].Id, DailyRate = 100.00 },
        new CarPricingRule { Id = Guid.NewGuid(), CarModelId = CarModels[2].Id, DailyRate = 170.00 },
        new CarPricingRule { Id = Guid.NewGuid(), CarModelId = CarModels[3].Id, DailyRate = 120.00 }
    };

    public static readonly List<Car> Cars = new()
    {
        // Model S cars
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "1234TMS", CarModelId = CarModels[0].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "5678TMS", CarModelId = CarModels[0].Id },
        
        // Model 3 cars
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "1234TM3", CarModelId = CarModels[1].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "5678TM3", CarModelId = CarModels[1].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "9012TM3", CarModelId = CarModels[1].Id },
        
        // Model X cars
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "1234TMX", CarModelId = CarModels[2].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "5678TMX", CarModelId = CarModels[2].Id },
        
        // Model Y cars
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "1234TMY", CarModelId = CarModels[3].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "5678TMY", CarModelId = CarModels[3].Id }
    };

    public static readonly List<Reservation> Reservations = new()
    {
        new Reservation(
            car: Cars[0], // Model S at Airport
            customerName: "John Smith",
            customerEmail: "john.smith@email.com",
            startDate: new DateTime(2025, 4, 20),
            endDate: new DateTime(2025, 4, 25),
            totalCost: 750.00, // 5 days * 150.00
            pickupLocation: Locations[0],
            returnLocation: Locations[0],
            reservationStatus: ReservationStatus.Confirmed
        ),
        new Reservation(
            car: Cars[2], // Model 3 at Airport
            customerName: "Maria Garcia",
            customerEmail: "maria.garcia@email.com",
            startDate: new DateTime(2025, 5, 1),
            endDate: new DateTime(2025, 5, 5),
            totalCost: 400.00, // 4 days * 100.00
            pickupLocation: Locations[0],
            returnLocation: Locations[1],
            reservationStatus: ReservationStatus.Confirmed
        ),
        new Reservation(
            car: Cars[5], // Model X at Airport
            customerName: "David Brown",
            customerEmail: "david.brown@email.com",
            startDate: new DateTime(2025, 6, 15),
            endDate: new DateTime(2025, 6, 20),
            totalCost: 850.00, // 5 days * 170.00
            pickupLocation: Locations[0],
            returnLocation: Locations[2],
            reservationStatus: ReservationStatus.Planned
        )
    };
} 