using LoyaltyService.Data;
using LoyaltyService.Domain;
using LoyaltyService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyService.Controllers;

[ApiController]
[Route("api/v1/loyalty")]
public class LoyaltyController(LoyaltyDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<LoyaltyResponse>> Get([FromHeader(Name = "X-User-Name")] string username)
    {
        var loyalty = await GetOrCreateAsync(username);
        return Ok(ToResponse(loyalty));
    }

    [HttpPost("increment")]
    public async Task<ActionResult<LoyaltyResponse>> Increment([FromHeader(Name = "X-User-Name")] string username)
    {
        var loyalty = await GetOrCreateAsync(username);
        loyalty.ReservationCount++;
        ApplyRules(loyalty);
        await db.SaveChangesAsync();
        return Ok(ToResponse(loyalty));
    }

    [HttpPost("decrement")]
    public async Task<ActionResult<LoyaltyResponse>> Decrement([FromHeader(Name = "X-User-Name")] string username)
    {
        var loyalty = await GetOrCreateAsync(username);
        loyalty.ReservationCount = Math.Max(0, loyalty.ReservationCount - 1);
        ApplyRules(loyalty);
        await db.SaveChangesAsync();
        return Ok(ToResponse(loyalty));
    }

    private async Task<Loyalty> GetOrCreateAsync(string username)
    {
        var loyalty = await db.Loyalties.FirstOrDefaultAsync(l => l.Username == username);
        if (loyalty is not null)
            return loyalty;

        loyalty = new Loyalty { Username = username, ReservationCount = 0 };
        ApplyRules(loyalty);
        db.Loyalties.Add(loyalty);
        await db.SaveChangesAsync();
        return loyalty;
    }

    private static void ApplyRules(Loyalty loyalty)
    {
        (loyalty.Status, loyalty.Discount) = LoyaltyRules.ByCount(loyalty.ReservationCount);
    }

    private static LoyaltyResponse ToResponse(Loyalty l) => new(l.Status, l.Discount, l.ReservationCount);
}