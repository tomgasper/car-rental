using CarRental.src.Infrastructure;
using CarRental.src.Models;
using CarRental.src.Repositories.Interfaces;

sealed class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _dbContext;

    public List<Reservation> GetByRange()
    {
        throw new NotImplementedException();
    }
}