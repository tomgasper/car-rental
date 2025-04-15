using CarRental.src.DTOs.Reservation;
using CarRental.src.Models;
using CarRental.src.Services.Interfaces;

sealed class ReservationService : IReservationService
{
    private readonly IPricingService _pricingService;

    public ReservationService(IPricingService pricingService)
    {
        _pricingService = pricingService;
    }

    public void ReserveCar(ReservationRequest request)
    {
        // Check availability for the dates
        // Get all cars that match the request
        // Check which are free
    }
}