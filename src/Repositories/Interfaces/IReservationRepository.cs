using CarRental.src.Models;

namespace CarRental.src.Repositories.Interfaces;

public interface IReservationRepository
{
    void AddReservation(Reservation reservation);
    public Task<List<Reservation>> GetByModelAndDate(string carModel, DateTime startDate, DateTime endDate);
}