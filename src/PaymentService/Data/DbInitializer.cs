namespace PaymentService.Data;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

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
    }
}