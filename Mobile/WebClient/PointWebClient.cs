using Mobile.Models;
using System.Net.Http.Headers;

namespace Mobile.WebClient;

public class PointWebClient : IPointWebClient<Models.Point>
{
    private HttpClient _client;

    public PointWebClient(string token)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api-contpass.azurewebsites.net/");
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Add("token", token);
    }

    public async Task<List<Models.Point>> ListToday()
    {
        HttpResponseMessage response = await _client.GetAsync("point/listtodaybyperiod");

        return await response.Content.ReadAsAsync<List<Models.Point>>();
    }
}
