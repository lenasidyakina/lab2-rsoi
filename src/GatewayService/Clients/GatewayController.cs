using GatewayService.Clients;
using GatewayService.Domain;
using GatewayService.Models;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Controllers;

[ApiController]
[Route("api/v1")]
public class GatewayController(
    ReservationClient reservationClient,
    PaymentClient paymentClient,
    LoyaltyClient loyaltyClient) : ControllerBase
{
    [HttpGet("hotels")]
    public async Task<ActionResult<PaginationResponse>> GetHotels([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        return Ok(await reservationClient.GetHotelsAsync(page, size));
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserInfoResponse>> GetMe([FromHeader(Name = "X-User-Name")] string username)
    {
        var reservations = await GetUserReservationsAsync(username);
        var loyalty = await loyaltyClient.GetAsync(username);
        return Ok(new UserInfoResponse(reservations, loyalty));
    }

    [HttpGet("reservations")]
    public async Task<ActionResult<List<ReservationResponse>>> GetReservations(
        [FromHeader(Name = "X-User-Name")] string username)
    {
        return Ok(await GetUserReservationsAsync(username));
    }

    [HttpGet("reservations/{reservationUid:guid}")]
    public async Task<ActionResult<ReservationResponse>> GetReservation(
        Guid reservationUid, [FromHeader(Name = "X-User-Name")] string username)
    {
        var reservation = await reservationClient.GetReservationAsync(reservationUid, username);
        if (reservation is null)
            return NotFound(new ErrorResponse("Reservation not found"));

        var payment = await paymentClient.GetAsync(reservation.PaymentUid);
        return Ok(ToResponse(reservation, payment));
    }

    [HttpPost("reservations")]
    public async Task<ActionResult<CreateReservationResponse>> CreateReservation(
        [FromHeader(Name = "X-User-Name")] string username, [FromBody] CreateReservationRequest request)
    {
        if (request.EndDate <= request.StartDate)
            return BadRequest(new ValidationErrorResponse("Validation failed",
                [new ErrorDescription("endDate", "endDate must be after startDate")]));

        var hotel = await reservationClient.GetHotelAsync(request.HotelUid);
        if (hotel is null)
            return NotFound(new ErrorResponse("Hotel not found"));

        var nights = Pricing.Nights(request.StartDate, request.EndDate);
        var loyalty = await loyaltyClient.GetAsync(username);
        var price = Pricing.Calculate(hotel.Price, nights, loyalty.Discount);

        var payment = await paymentClient.CreateAsync(price);

        await loyaltyClient.IncrementAsync(username);
        
        var reservation = await reservationClient.CreateReservationAsync(username,
            new CreateReservationDto(request.HotelUid, payment.PaymentUid, request.StartDate, request.EndDate));

        return Ok(new CreateReservationResponse(
            reservation.ReservationUid,
            hotel.HotelUid,
            reservation.StartDate,
            reservation.EndDate,
            loyalty.Discount,
            reservation.Status,
            new PaymentInfo(payment.Status, payment.Price)));
    }

    [HttpDelete("reservations/{reservationUid:guid}")]
    public async Task<IActionResult> CancelReservation(
        Guid reservationUid, [FromHeader(Name = "X-User-Name")] string username)
    {
        var reservation = await reservationClient.GetReservationAsync(reservationUid, username);
        if (reservation is null)
            return NotFound(new ErrorResponse("Reservation not found"));

        await reservationClient.CancelReservationAsync(reservationUid, username);
        await paymentClient.CancelAsync(reservation.PaymentUid);
        await loyaltyClient.DecrementAsync(username);

        return NoContent();
    }

    [HttpGet("loyalty")]
    public async Task<ActionResult<LoyaltyInfoResponse>> GetLoyalty([FromHeader(Name = "X-User-Name")] string username)
    {
        return Ok(await loyaltyClient.GetAsync(username));
    }

    private async Task<List<ReservationResponse>> GetUserReservationsAsync(string username)
    {
        var reservations = await reservationClient.GetReservationsAsync(username);
        var result = new List<ReservationResponse>();
        foreach (var reservation in reservations)
        {
            var payment = await paymentClient.GetAsync(reservation.PaymentUid);
            result.Add(ToResponse(reservation, payment));
        }
        return result;
    }

    private static ReservationResponse ToResponse(ReservationDto r, PaymentDto p) => new(
        r.ReservationUid,
        new HotelInfo(
            r.Hotel.HotelUid,
            r.Hotel.Name,
            $"{r.Hotel.Country}, {r.Hotel.City}, {r.Hotel.Address}",
            r.Hotel.Stars),
        r.StartDate,
        r.EndDate,
        r.Status,
        new PaymentInfo(p.Status, p.Price));
}