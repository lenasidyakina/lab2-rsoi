using LoyaltyService.Models;

namespace LoyaltyService.Data;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LoyaltyDbContext>();

        for (var attempt = 1; ; attempt++)
        {
            try
            {
                db.Database.EnsureCreated();
                break;
            }
            catch when (attempt < 10)
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
        }

        if (!db.Loyalties.Any(l => l.Username == "Test Max"))
        {
            db.Loyalties.Add(new Loyalty
            {
                Username = "Test Max",
                ReservationCount = 25,
                Status = "GOLD",
                Discount = 10
            });
            db.SaveChanges();
        }
    }
}