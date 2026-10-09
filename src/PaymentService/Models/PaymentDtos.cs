namespace PaymentService.Models;

public record CreatePaymentRequest(int Price);

public record PaymentResponse(Guid PaymentUid, string Status, int Price);