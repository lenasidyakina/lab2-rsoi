using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationService.Data;
using ReservationService.Models;

namespace ReservationService.Controllers;

[ApiController]
[Route("api/v1/hotels")]
public class HotelsController(ReservationDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginationResponse>> GetHotels([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 10;

        var total = await db.Hotels.CountAsync();
        var hotels = await db.Hotels
            .OrderBy(h => h.Id)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return Ok(new PaginationResponse(page, size, total, hotels.Select(h => h.ToResponse()).ToList()));
    }

    [HttpGet("{hotelUid:guid}")]
    public async Task<ActionResult<HotelResponse>> GetHotel(Guid hotelUid)
    {
        var hotel = await db.Hotels.FirstOrDefaultAsync(h => h.HotelUid == hotelUid);
        return hotel is null ? NotFound() : Ok(hotel.ToResponse());
    }
}