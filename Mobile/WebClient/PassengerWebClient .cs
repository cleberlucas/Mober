using Mobile.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.WebPages;

namespace Mobile.WebClient;
public class PassengerWebClient : IPassengerWebClient<Passenger>
{
    private HttpClient _client;

    public PassengerWebClient(string token)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api-contpass.azurewebsites.net/");
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Add("token", token);
    }


    public async Task<List<Passenger>> List(Passenger passenger)
    {
        string dateTime = passenger.DateTime == DateTime.MinValue ? "" : $"datetime={passenger.DateTime.ToString("yyyy-MM-dd")}";
        string period = passenger.Period.IsEmpty() ? "" : $"period={passenger.Period}";

        HttpResponseMessage response = await _client.GetAsync($"passenger/list?{dateTime}&{period}");

        return await response.Content.ReadAsAsync<List<Passenger>>();
    }

    public async Task<bool> ConfirmPresenceBack(Passenger passenger)
    {
        HttpResponseMessage response = await _client.PutAsync("passenger/confirmpresenceback", new StringContent(JsonConvert.SerializeObject(passenger), Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ConfirmPresenceGone(Passenger passenger)
    {
        HttpResponseMessage response = await _client.PutAsync("passenger/confirmpresencegone", new StringContent(JsonConvert.SerializeObject(passenger), Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Reserve(Passenger passenger)
    {
        HttpResponseMessage response = await _client.PostAsync("passenger/reserve", new StringContent(JsonConvert.SerializeObject(passenger), Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }
}
