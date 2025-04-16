using CarRental.src.Infrastructure.DataSeeding;
using CarRental.src.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.src.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the Car entity
        modelBuilder.Entity<Car>()
            .HasOne(c => c.Location)
            .WithMany()
            .HasForeignKey(c => c.LocationId)
            .IsRequired();
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<CarPricingRule> CarPricingRules { get; set; }
    public DbSet<Location> Locations { get; set; }
}