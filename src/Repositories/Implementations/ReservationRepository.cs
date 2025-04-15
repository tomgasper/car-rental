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
}