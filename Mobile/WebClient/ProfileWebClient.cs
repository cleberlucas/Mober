using Mobile.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;



namespace Mobile.WebClient;

public class ProfileWebClient : IProfileWebClient<Profile>
{
    private HttpClient _client;

    public ProfileWebClient(string token)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api-contpass.azurewebsites.net/");
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Add("token", token);
    }


    public async Task<Profile> View()
    {
        HttpResponseMessage response = await _client.GetAsync("profile/view");

        return await response.Content.ReadAsAsync<Profile>();

    }
}
