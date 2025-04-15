using Microsoft.AspNetCore.Mvc;

[Route("api/reservations")]
sealed class ReservationController : ApiController {
    [HttpPost]
    public async Task<IActionResult> MakeReservation()
    {
        
    }
}