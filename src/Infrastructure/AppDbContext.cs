using CarRental.src.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.src.Infrastructure;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Car> Cars { get;set;}
    public DbSet<Reservation> Reservations { get;set; }
}