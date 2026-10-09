namespace ReservationService.Models;

public static class Mapper
{
    public static HotelResponse ToResponse(this Hotel h) =>
        new(h.HotelUid, h.Name, h.Country, h.City, h.Address, h.Stars, h.Price);

    public static ReservationResponse ToResponse(this Reservation r) =>
        new(r.ReservationUid,
            r.Username,
            r.PaymentUid,
            r.Status,
            DateOnly.FromDateTime(r.StartDate),
            DateOnly.FromDateTime(r.EndDate),
            r.Hotel.ToResponse());

    public static DateTime ToUtcDateTime(this DateOnly date) =>
        date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
}