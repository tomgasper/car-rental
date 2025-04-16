using CarRental.src.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.src.Infrastructure;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial data
        Location PalmaAirportLocation = new("PAP", "Palma Airport", "Llevant de Palma District, 07611 Palma");
        Location PalmaCityCenter = new("PCC", "Palma City Center", "Carrer de la Missió, 15, Centre, 07003 Palma, Illes Balears");
        Location ManacorLocation = new("MAN", "Manacor", "Carrer d'es Convent, 11, 07500 Manacor, Illes Balears");

        modelBuilder.Entity<Car>().HasData(
            new Car { Id = new Guid(), CarModel = CarModel.ModelS, Location = PalmaAirportLocation}
        );
    }

    public DbSet<Car> Cars { get;set;}
    public DbSet<Reservation> Reservations { get;set; }
    public DbSet<CarPricingRule> CarPricingRules { get;set; }
    public DbSet<Location> Locations { get;set; }
}