namespace LoyaltyService.Domain;

public static class LoyaltyRules
{
    public static (string Status, int Discount) ByCount(int reservationCount) =>
        reservationCount >= 20 ? ("GOLD", 10)
        : reservationCount >= 10 ? ("SILVER", 7)
        : ("BRONZE", 5);
}