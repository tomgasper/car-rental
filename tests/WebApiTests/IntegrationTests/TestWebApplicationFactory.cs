using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CarRental.src.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using CarRental.src.Infrastructure.DataSeeding;
using Microsoft.Data.Sqlite;

namespace WebApiTests.IntegrationTests;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly SqliteConnection _connection;

    public TestWebApplicationFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // remove the app's dbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // SQLite database for testing
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            var serviceProvider = services.BuildServiceProvider();

            // scope to obtain a reference to the database context
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                db.Database.EnsureCreated();

                // seed test data
                if (!db.Locations.Any() || !db.CarPricingRules.Any() || !db.CarModels.Any() || !db.Cars.Any() || !db.Reservations.Any())
                {
                    db.Locations.AddRange(InitialData.Locations);
                    db.SaveChanges();

                    db.CarPricingRules.AddRange(InitialData.PricingRules);
                    db.SaveChanges();

                    db.CarModels.AddRange(InitialData.CarModels);
                    db.SaveChanges();

                    db.Cars.AddRange(InitialData.Cars);
                    db.SaveChanges();

                    db.Reservations.AddRange(InitialData.Reservations);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting up the test database.", ex);
            }
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection.Dispose();
        }
    }
} 