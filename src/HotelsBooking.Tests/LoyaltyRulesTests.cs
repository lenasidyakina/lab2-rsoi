using LoyaltyService.Domain;

namespace HotelsBooking.Tests;

public class LoyaltyRulesTests
{
    [Fact]
    public void NoBookings_IsBronze()
    {
        Assert.Equal(("BRONZE", 5), LoyaltyRules.ByCount(0));
    }

    [Fact]
    public void NineBookings_IsStillBronze()
    {
        Assert.Equal(("BRONZE", 5), LoyaltyRules.ByCount(9));
    }

    [Fact]
    public void TenBookings_IsSilver()
    {
        Assert.Equal(("SILVER", 7), LoyaltyRules.ByCount(10));
    }

    [Fact]
    public void NineteenBookings_IsStillSilver()
    {
        Assert.Equal(("SILVER", 7), LoyaltyRules.ByCount(19));
    }

    [Fact]
    public void TwentyBookings_IsGold()
    {
        Assert.Equal(("GOLD", 10), LoyaltyRules.ByCount(20));
    }

    [Fact]
    public void TwentyFiveBookings_IsGold()
    {
        Assert.Equal(("GOLD", 10), LoyaltyRules.ByCount(25));
    }
}
