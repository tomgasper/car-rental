using CarRental.src.DTOs.Reservation;
using FluentResults;

namespace CarRental.src.Services.Interfaces;
interface IReservationService
{
    Result<ReservationResponse> ReserveCar(ReservationRequest reservationRequest);
}