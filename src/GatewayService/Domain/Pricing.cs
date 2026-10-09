namespace GatewayService.Domain;

public static class Pricing
{
    public static int Nights(DateOnly startDate, DateOnly endDate) =>
        endDate.DayNumber - startDate.DayNumber;

    public static int Calculate(int pricePerNight, int nights, int discount) =>
        pricePerNight * nights * (100 - discount) / 100;
}