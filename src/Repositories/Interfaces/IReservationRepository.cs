using CarRental.src.Models;

namespace CarRental.src.Repositories.Interfaces;

interface IReservationRepository
{
    void AddReservation(Reservation reservation);
}