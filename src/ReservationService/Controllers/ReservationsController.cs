using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.Models;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/v1/reservations")]
public class ReservationsController(ReservationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ReservationResponse>>> GetAll([FromHeader(Name = "X-User-Name")] string username)
    {
        var reservations = await db.Reservations
            .Include(r => r.Hotel)
            .Where(r => r.Username == username)
            .OrderBy(r => r.Id)
            .ToListAsync();

        return Ok(reservations.Select(r => r.ToResponse()).ToList());
    }

    [HttpGet("{reservationUid:guid}")]
    public async Task<ActionResult<ReservationResponse>> Get(
        Guid reservationUid, [FromHeader(Name = "X-User-Name")] string username)
    {
        var reservation = await db.Reservations
            .Include(r => r.Hotel)
            .FirstOrDefaultAsync(r => r.ReservationUid == reservationUid && r.Username == username);

        return reservation is null ? NotFound() : Ok(reservation.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponse>> Create(
        [FromHeader(Name = "X-User-Name")] string username, [FromBody] CreateReservationRequest request)
    {
        var hotel = await db.Hotels.FirstOrDefaultAsync(h => h.HotelUid == request.HotelUid);
        if (hotel is null)
            return NotFound();

        var reservation = new Reservation
        {
            ReservationUid = Guid.NewGuid(),
            Username = username,
            PaymentUid = request.PaymentUid,
            HotelId = hotel.Id,
            Hotel = hotel,
            Status = "PAID",
            StartDate = request.StartDate.ToUtcDateTime(),
            EndDate = request.EndDate.ToUtcDateTime()
        };
        db.Reservations.Add(reservation);
        await db.SaveChangesAsync();

        return Ok(reservation.ToResponse());
    }

    [HttpDelete("{reservationUid:guid}")]
    public async Task<IActionResult> Cancel(Guid reservationUid, [FromHeader(Name = "X-User-Name")] string username)
    {
        var reservation = await db.Reservations
            .FirstOrDefaultAsync(r => r.ReservationUid == reservationUid && r.Username == username);
        if (reservation is null)
            return NotFound();

        reservation.Status = "CANCELED";
        await db.SaveChangesAsync();
        return NoContent();
    }
}