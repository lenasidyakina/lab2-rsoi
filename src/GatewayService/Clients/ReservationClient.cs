using System.Net;
using GatewayService.Models;

namespace GatewayService.Clients;

public class ReservationClient(HttpClient http)
{
    public async Task<PaginationResponse> GetHotelsAsync(int page, int size) =>
        (await http.GetFromJsonAsync<PaginationResponse>($"/api/v1/hotels?page={page}&size={size}"))!;

    public async Task<HotelResponse?> GetHotelAsync(Guid hotelUid)
    {
        using var response = await http.GetAsync($"/api/v1/hotels/{hotelUid}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<HotelResponse>();
    }

    public async Task<List<ReservationDto>> GetReservationsAsync(string username)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/reservations");
        request.Headers.Add("X-User-Name", username);

        using var response = await http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<ReservationDto>>())!;
    }

    public async Task<ReservationDto?> GetReservationAsync(Guid reservationUid, string username)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/reservations/{reservationUid}");
        request.Headers.Add("X-User-Name", username);

        using var response = await http.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReservationDto>();
    }

    public async Task<ReservationDto> CreateReservationAsync(string username, CreateReservationDto body)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/reservations");
        request.Headers.Add("X-User-Name", username);
        request.Content = JsonContent.Create(body);

        using var response = await http.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ReservationDto>())!;
    }

    public async Task CancelReservationAsync(Guid reservationUid, string username)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/reservations/{reservationUid}");
        request.Headers.Add("X-User-Name", username);

        using var response = await http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}