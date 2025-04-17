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

        modelBuilder.Entity<CarModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            
            entity.HasMany(e => e.Cars)
                  .WithOne(e => e.CarModel)
                  .HasForeignKey(e => e.CarModelId)
                  .IsRequired();
        });
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<CarModel> CarModels { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<CarPricingRule> CarPricingRules { get; set; }
    public DbSet<Location> Locations { get; set; }
}