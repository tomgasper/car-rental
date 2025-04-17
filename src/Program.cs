using CarRental.src.Infrastructure;
using CarRental.src.Infrastructure.DataSeeding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder
                .WithOrigins(
                    "http://localhost:5173", // vite dev server
                    "http://localhost:3000"  // production/docker
                )
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

// Exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddServices();
builder.Services.AddRepositories();

var app = builder.Build();

app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseExceptionHandler();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Seed sample data
// This is only for presentation purposes, 
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DatabaseSeeder.SeedDatabase(app.Services);
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
});

app.Run();

// Needed for integration tests
public partial class Program { }