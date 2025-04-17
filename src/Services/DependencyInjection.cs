using CarRental.src.DTOs.Reservation;
using CarRental.src.Services.Implementations;
using CarRental.src.Services.Interfaces;
using FluentValidation;

public static class ServiceDependencyInjection {
    public static IServiceCollection AddServices(this IServiceCollection services) {
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IPricingService, PricingService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IValidator<ReservationRequest>, ReservationRequestValidator>();
        return services;
    }
}