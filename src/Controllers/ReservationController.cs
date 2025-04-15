using CarRental.src.DTOs.Reservation;
using Microsoft.AspNetCore.Mvc;

[Route("api/reservations")]
sealed class ReservationController : ApiController {
    private readonly ReservationService _reservationService;

    public ReservationController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> MakeReservation([FromBody] ReservationRequest request)
    {
        ReservationResponse response = await _reservationService.ReserveCar(request);

        if (response.IsFailed)
        {
            return Problem(response.Errors);
        }

        return Ok(response.Value);
    }
}