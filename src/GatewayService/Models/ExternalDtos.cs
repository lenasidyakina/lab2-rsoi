namespace GatewayService.Models;

public record HotelInfo(Guid HotelUid, string Name, string FullAddress, int? Stars);

public record PaymentInfo(string Status, int Price);

public record ReservationResponse(
    Guid ReservationUid,
    HotelInfo Hotel,
    DateOnly StartDate,
    DateOnly EndDate,
    string Status,
    PaymentInfo Payment);

public record UserInfoResponse(List<ReservationResponse> Reservations, LoyaltyInfoResponse Loyalty);

public record CreateReservationRequest(Guid HotelUid, DateOnly StartDate, DateOnly EndDate);

public record CreateReservationResponse(
    Guid ReservationUid,
    Guid HotelUid,
    DateOnly StartDate,
    DateOnly EndDate,
    int Discount,
    string Status,
    PaymentInfo Payment);

public record ErrorResponse(string Message);

public record ErrorDescription(string Field, string Error);

public record ValidationErrorResponse(string Message, List<ErrorDescription> Errors);