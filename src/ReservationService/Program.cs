using Microsoft.EntityFrameworkCore;
using ReservationService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

DbInitializer.Initialize(app.Services);

app.MapGet("/manage/health", () => Results.Ok());
app.MapControllers();

app.Run();