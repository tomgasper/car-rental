using CarRental.src.Models;

namespace CarRental.src.Services.Interfaces;
public interface IReservationService
{
    void ReserveCar(CarModel carModel, DateTime startDate, DateTime endDate);
}