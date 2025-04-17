using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarRental.src.Infrastructure.DataSeeding;

public static class DatabaseSeeder
{
    public static async Task SeedDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Apply migrations
        await context.Database.EnsureCreatedAsync();

        // Check if we need to seed
        if (!context.Locations.Any() && !context.CarPricingRules.Any() && !context.Cars.Any() && !context.CarModels.Any() && !context.Reservations.Any())
        {
            await context.Locations.AddRangeAsync(InitialData.Locations);
            await context.SaveChangesAsync();

            await context.CarPricingRules.AddRangeAsync(InitialData.PricingRules);
            await context.SaveChangesAsync();

            await context.CarModels.AddRangeAsync(InitialData.CarModels);
            await context.SaveChangesAsync();

            await context.Cars.AddRangeAsync(InitialData.Cars);
            await context.SaveChangesAsync();

            await context.Reservations.AddRangeAsync(InitialData.Reservations);
            await context.SaveChangesAsync();
        }
    }
} 