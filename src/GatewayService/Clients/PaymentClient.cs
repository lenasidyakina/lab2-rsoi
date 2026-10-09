using GatewayService.Models;

namespace GatewayService.Clients;

public class PaymentClient(HttpClient http)
{
    public async Task<PaymentDto> CreateAsync(int price)
    {
        using var response = await http.PostAsJsonAsync("/api/v1/payments", new { price });
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PaymentDto>())!;
    }

    public async Task<PaymentDto> GetAsync(Guid paymentUid) =>
        (await http.GetFromJsonAsync<PaymentDto>($"/api/v1/payments/{paymentUid}"))!;

    public async Task CancelAsync(Guid paymentUid)
    {
        using var response = await http.DeleteAsync($"/api/v1/payments/{paymentUid}");
        response.EnsureSuccessStatusCode();
    }
}