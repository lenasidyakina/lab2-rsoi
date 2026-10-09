namespace ReservationService.Models;

public record HotelResponse(
    Guid HotelUid, string Name, string Country, string City, string Address, int? Stars, int Price);

public record PaginationResponse(int Page, int PageSize, int TotalElements, List<HotelResponse> Items);

public record CreateReservationRequest(Guid HotelUid, Guid PaymentUid, DateOnly StartDate, DateOnly EndDate);

public record ReservationResponse(
    Guid ReservationUid,
    string Username,
    Guid PaymentUid,
    string Status,
    DateOnly StartDate,
    DateOnly EndDate,
    HotelResponse Hotel);