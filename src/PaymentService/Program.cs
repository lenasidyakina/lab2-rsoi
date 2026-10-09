using Microsoft.EntityFrameworkCore;
using PaymentService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

DbInitializer.Initialize(app.Services);

app.MapGet("/manage/health", () => Results.Ok());
app.MapControllers();

app.Run();