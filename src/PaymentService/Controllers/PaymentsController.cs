using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/v1/payments")]
public class PaymentsController(PaymentDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<PaymentResponse>> Create([FromBody] CreatePaymentRequest request)
    {
        var payment = new Payment
        {
            PaymentUid = Guid.NewGuid(),
            Status = "PAID",
            Price = request.Price
        };
        db.Payments.Add(payment);
        await db.SaveChangesAsync();
        return Ok(ToResponse(payment));
    }

    [HttpGet("{paymentUid:guid}")]
    public async Task<ActionResult<PaymentResponse>> Get(Guid paymentUid)
    {
        var payment = await db.Payments.FirstOrDefaultAsync(p => p.PaymentUid == paymentUid);
        return payment is null ? NotFound() : Ok(ToResponse(payment));
    }

    [HttpDelete("{paymentUid:guid}")]
    public async Task<IActionResult> Cancel(Guid paymentUid)
    {
        var payment = await db.Payments.FirstOrDefaultAsync(p => p.PaymentUid == paymentUid);
        if (payment is null)
            return NotFound();

        payment.Status = "CANCELED";
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static PaymentResponse ToResponse(Payment p) => new(p.PaymentUid, p.Status, p.Price);
}