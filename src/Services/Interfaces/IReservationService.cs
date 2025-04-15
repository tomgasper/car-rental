using CarRental.src.DTOs.Reservation;

namespace CarRental.src.Services.Interfaces;
interface IReservationService
{
    void ReserveCar(ReservationRequest reservationRequest);
}