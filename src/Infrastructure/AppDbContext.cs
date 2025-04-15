using Microsoft.EntityFrameworkCore;

namespace CarRental.src.Infrastructure;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) {}
}