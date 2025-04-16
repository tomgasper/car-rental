using CarRental.src.Services.Implementations;
using CarRental.src.Services.Interfaces;

public static class ServiceDependencyInjection {
    public static IServiceCollection AddServices(this IServiceCollection services) {
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IPricingService, PricingService>();
        
        return services;
    }
}