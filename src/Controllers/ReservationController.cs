using CarRental.src.DTOs.Reservation;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

[Route("api/reservations")]
public sealed class ReservationController : ApiController {
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService reservationService)
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