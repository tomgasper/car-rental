using CarRental.src.DTOs.Reservation;
using FluentResults;

namespace CarRental.src.Services.Interfaces;
public interface IReservationService
{
    Task<Result<ReservationResponse>> ReserveCar(ReservationRequest reservationRequest);
}