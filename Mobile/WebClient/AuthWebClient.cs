using Mobile.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;



namespace Mobile.WebClient;

public class AuthWebClient : IAuthWebClient<Auth>
{
    private HttpClient _client;

    public AuthWebClient()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api-contpass.azurewebsites.net/");
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

    }


    public async Task<string> Token(Auth login)
    {
        HttpResponseMessage response = await _client.PostAsync("auth/gettoken", new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsAsync<string>();
        }

        throw new Exception(await response.Content.ReadAsAsync<string>());
    }
}
