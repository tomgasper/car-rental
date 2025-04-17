using CarRental.src.Infrastructure;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

sealed class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _dbContext;

    public ReservationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddReservation(Reservation reservation)
    {
        _dbContext.Reservations.Add(reservation);
    }

    public async Task<List<Reservation>> GetByModelAndDate(string carModelCode, DateTime startDate, DateTime endDate)
    {
        return await _dbContext.Reservations
            .Include(reservation => reservation.Car)
                .ThenInclude( car => car.CarModel)
            .Where(reservation => 
                reservation.Car.CarModel.Code == carModelCode &&
                (reservation.ReservationStatus == ReservationStatus.Confirmed || 
                    reservation.ReservationStatus == ReservationStatus.Planned) &&
                ((startDate >= reservation.StartDate && startDate <= reservation.EndDate) ||
                (endDate >= reservation.StartDate && endDate <= reservation.EndDate)))
            .ToListAsync();
    }
}