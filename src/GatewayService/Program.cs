using GatewayService.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient<ReservationClient>(c =>
    c.BaseAddress = new Uri(builder.Configuration["Services:Reservation"]!));
builder.Services.AddHttpClient<PaymentClient>(c =>
    c.BaseAddress = new Uri(builder.Configuration["Services:Payment"]!));
builder.Services.AddHttpClient<LoyaltyClient>(c =>
    c.BaseAddress = new Uri(builder.Configuration["Services:Loyalty"]!));

var app = builder.Build();

app.MapGet("/manage/health", () => Results.Ok());
app.MapControllers();

app.Run();