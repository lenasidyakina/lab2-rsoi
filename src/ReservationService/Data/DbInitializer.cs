using ReservationService.Models;

namespace ReservationService.Data;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();

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

        var testHotelUid = Guid.Parse("049161bb-badd-4fa8-9d90-87c9a82b0668");
        if (!db.Hotels.Any(h => h.HotelUid == testHotelUid))
        {
            db.Hotels.Add(new Hotel
            {
                HotelUid = testHotelUid,
                Name = "Ararat Park Hyatt Moscow",
                Country = "Россия",
                City = "Москва",
                Address = "Неглинная ул., 4",
                Stars = 5,
                Price = 10000
            });
            db.SaveChanges();
        }
    }
}