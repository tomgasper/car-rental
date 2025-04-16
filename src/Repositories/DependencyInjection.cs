using CarRental.src.Repositories.Implementations;
using CarRental.src.Repositories.Interfaces;

public static class RepositoryDependencyInjection {
    public static IServiceCollection AddRepositories(this IServiceCollection services) {
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<ICarPricingRuleRepository, CarPricingRepository>();

        return services;
    }
}