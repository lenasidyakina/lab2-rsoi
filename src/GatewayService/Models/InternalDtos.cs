namespace GatewayService.Models;

public record HotelResponse(
    Guid HotelUid, string Name, string Country, string City, string Address, int? Stars, int Price);

public record PaginationResponse(int Page, int PageSize, int TotalElements, List<HotelResponse> Items);

public record ReservationDto(
    Guid ReservationUid,
    string Username,
    Guid PaymentUid,
    string Status,
    DateOnly StartDate,
    DateOnly EndDate,
    HotelResponse Hotel);

public record CreateReservationDto(Guid HotelUid, Guid PaymentUid, DateOnly StartDate, DateOnly EndDate);

public record PaymentDto(Guid PaymentUid, string Status, int Price);

public record LoyaltyInfoResponse(string Status, int Discount, int ReservationCount);