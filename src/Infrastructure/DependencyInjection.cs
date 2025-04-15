using CarRental.src.Infrastructure;
using CarRental.src.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

public static class InfrastructureDependencyInjection {
    public static void AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddUnitOfWork();
        services.AddDbContext(configuration);
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services) {
        return services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        // Entity Framework Configuration
        services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}