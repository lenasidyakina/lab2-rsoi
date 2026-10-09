using GatewayService.Domain;
using LoyaltyService.Domain;

namespace HotelsBooking.Tests;

public class BookingEdgeCaseTests
{
    [Fact]
    public void LeapDay_CountsAsExtraNight()
    {
        Assert.Equal(2, Pricing.Nights(new DateOnly(2024, 2, 28), new DateOnly(2024, 3, 1)));
        Assert.Equal(1, Pricing.Nights(new DateOnly(2023, 2, 28), new DateOnly(2023, 3, 1)));
    }

    [Fact]
    public void NewYearsEve_IsOneNight()
    {
        Assert.Equal(1, Pricing.Nights(new DateOnly(2021, 12, 31), new DateOnly(2022, 1, 1)));
    }

    [Fact]
    public void DaylightSavingSwitch_DoesNotChangeNightCount()
    {
        Assert.Equal(2, Pricing.Nights(new DateOnly(2021, 3, 27), new DateOnly(2021, 3, 29)));
        Assert.Equal(2, Pricing.Nights(new DateOnly(2021, 10, 30), new DateOnly(2021, 11, 1)));
    }

    [Fact]
    public void WholeLeapYearStay_Is366Nights()
    {
        Assert.Equal(366, Pricing.Nights(new DateOnly(2024, 1, 1), new DateOnly(2025, 1, 1)));
    }

    [Fact]
    public void CheckInAndCheckOutSameDay_CostsNothing()
    {
        var day = new DateOnly(2021, 10, 8);
        var nights = Pricing.Nights(day, day);

        Assert.Equal(0, nights);
        Assert.Equal(0, Pricing.Calculate(10000, nights, 10));
    }

    [Fact]
    public void SwappedDates_GiveNegativeNights()
    {
        Assert.Equal(-3, Pricing.Nights(new DateOnly(2021, 10, 11), new DateOnly(2021, 10, 8)));
    }

    [Fact]
    public void OneRubleRoom_WithBronzeDiscount_RoundsDownToZero()
    {
        Assert.Equal(0, Pricing.Calculate(1, 1, 5));
    }

    [Fact]
    public void CheapRoom_WithSilverDiscount_DropsKopecks()
    {
        Assert.Equal(92, Pricing.Calculate(99, 1, 7));
    }

    [Fact]
    public void OddPrice_WithGoldDiscount_DropsKopecks()
    {
        Assert.Equal(9000, Pricing.Calculate(10001, 1, 10));
    }

    [Fact]
    public void Discount_IsAppliedToTotal_NotToEachNight()
    {
        var perNight = Pricing.Calculate(333, 1, 10) * 3;
        var total = Pricing.Calculate(333, 3, 10);

        Assert.Equal(897, perNight);
        Assert.Equal(899, total);
    }

    [Fact]
    public void HundredPercentDiscount_MakesStayFree()
    {
        Assert.Equal(0, Pricing.Calculate(10000, 3, 100));
    }

    [Fact]
    public void NewGuest_BecomesSilverExactlyOnTenthBooking()
    {
        var count = 0;
        var statuses = new List<string>();

        for (var i = 0; i < 10; i++)
        {
            count++;
            statuses.Add(LoyaltyRules.ByCount(count).Status);
        }

        Assert.All(statuses.Take(9), s => Assert.Equal("BRONZE", s));
        Assert.Equal("SILVER", statuses.Last());
    }

    [Fact]
    public void CancellingTenthBooking_DropsSilverBackToBronze()
    {
        Assert.Equal("SILVER", LoyaltyRules.ByCount(10).Status);
        Assert.Equal("BRONZE", LoyaltyRules.ByCount(10 - 1).Status);
    }

    [Fact]
    public void Discount_NeverDecreases_WhenGuestBooksMore()
    {
        var previous = LoyaltyRules.ByCount(0).Discount;

        for (var count = 1; count <= 100; count++)
        {
            var current = LoyaltyRules.ByCount(count).Discount;
            Assert.True(current >= previous, $"discount dropped at {count} bookings");
            previous = current;
        }
    }

    [Fact]
    public void NegativeBookingCount_IsTreatedAsBronze()
    {
        Assert.Equal(("BRONZE", 5), LoyaltyRules.ByCount(-1));
    }

    [Fact]
    public void MinimalPossibleBookingCount_IsTreatedAsBronze()
    {
        Assert.Equal(("BRONZE", 5), LoyaltyRules.ByCount(int.MinValue));
    }

    [Fact]
    public void AbsurdlyLoyalGuest_IsStillJustGold()
    {
        Assert.Equal(("GOLD", 10), LoyaltyRules.ByCount(int.MaxValue));
    }

    [Fact]
    public void TestMax_BooksAndCancels_StaysGold()
    {
        var count = 25;
        var (_, discount) = LoyaltyRules.ByCount(count);
        var nights = Pricing.Nights(new DateOnly(2021, 10, 8), new DateOnly(2021, 10, 11));

        Assert.Equal(27000, Pricing.Calculate(10000, nights, discount));

        count++;
        Assert.Equal("GOLD", LoyaltyRules.ByCount(count).Status);

        count--;
        Assert.Equal("GOLD", LoyaltyRules.ByCount(count).Status);
    }
}
