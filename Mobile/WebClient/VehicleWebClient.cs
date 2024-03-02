using Mobile.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;



namespace Mobile.WebClient;
public class VehicleWebClient : IVehicleWebClient<Vehicle>
{
    private HttpClient _client;

    public VehicleWebClient(string token)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api-contpass.azurewebsites.net/");
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Add("token", token);
    }

    public async Task<List<Vehicle>> List()
    {
        HttpResponseMessage response = await _client.GetAsync("vehicle/list");

        return await response.Content.ReadAsAsync<List<Vehicle>>();
    }

    public async Task<bool> UpdateLocation(Vehicle vehicle)
    {
        HttpResponseMessage response = await _client.PutAsync("vehicle/updatelocation", new StringContent(JsonConvert.SerializeObject(vehicle), Encoding.UTF8, "application/json"));

        return response.IsSuccessStatusCode;
    }
}
