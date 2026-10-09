using GatewayService.Domain;

namespace HotelsBooking.Tests;

public class PricingTests
{
    [Fact]
    public void Nights_IsDifferenceInDays()
    {
        Assert.Equal(3, Pricing.Nights(new DateOnly(2021, 10, 8), new DateOnly(2021, 10, 11)));
    }

    [Fact]
    public void ThreeNights_WithGoldDiscount()
    {
        Assert.Equal(27000, Pricing.Calculate(10000, 3, 10));
    }

    [Fact]
    public void ThreeNights_WithSilverDiscount()
    {
        Assert.Equal(27900, Pricing.Calculate(10000, 3, 7));
    }

    [Fact]
    public void ThreeNights_WithBronzeDiscount()
    {
        Assert.Equal(28500, Pricing.Calculate(10000, 3, 5));
    }

    [Fact]
    public void OneNight_WithoutDiscount()
    {
        Assert.Equal(1000, Pricing.Calculate(1000, 1, 0));
    }
}
