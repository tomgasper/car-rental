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

    public static readonly List<CarPricingRule> PricingRules = new()
    {
        new CarPricingRule { Id = Guid.NewGuid(), CarModel = CarModel.ModelS, DailyRate = 150.00 },
        new CarPricingRule { Id = Guid.NewGuid(), CarModel = CarModel.Model3, DailyRate = 100.00 },
        new CarPricingRule { Id = Guid.NewGuid(), CarModel = CarModel.ModelX, DailyRate = 170.00 },
        new CarPricingRule { Id = Guid.NewGuid(), CarModel = CarModel.ModelY, DailyRate = 120.00 }
    };

    public static readonly List<Car> Cars = new()
    {
        // Model S cars
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "1234TMS", CarModel = CarModel.ModelS, LocationId = Locations[0].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "5678TMS", CarModel = CarModel.ModelS, LocationId = Locations[1].Id },
        
        // Model 3 cars
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "1234TM3", CarModel = CarModel.Model3, LocationId = Locations[0].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "5678TM3", CarModel = CarModel.Model3, LocationId = Locations[1].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "9012TM3", CarModel = CarModel.Model3, LocationId = Locations[2].Id },
        
        // Model X cars
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "1234TMX", CarModel = CarModel.ModelX, LocationId = Locations[0].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "5678TMX", CarModel = CarModel.ModelX, LocationId = Locations[1].Id },
        
        // Model Y cars
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "1234TMY", CarModel = CarModel.ModelY, LocationId = Locations[0].Id },
        new Car { Id = Guid.NewGuid(), RegistrationNumber = "5678TMY", CarModel = CarModel.ModelY, LocationId = Locations[2].Id }
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