using CarRental.src.DTOs.Reservation;
using CarRental.src.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[Route("v1/api/reservations")]
public sealed class ReservationController : ApiController {
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> MakeReservation([FromBody] ReservationRequest request)
    {
        Result<ReservationResponse> response = await _reservationService.ReserveCar(request);

        if (response.IsFailed)
        {
            return Problem(response.Errors);
        }

        return Ok(response.Value);
    }
}