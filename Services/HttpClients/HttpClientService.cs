using Newtonsoft.Json;
using StarterApi.ApiModels.Login;
using System.Security.Authentication;
using System.Text;

namespace StarterApi.Services.HttpClients
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<string> Login(LoginRequest loginRequest)
        {
            string payload = JsonConvert.SerializeObject(loginRequest);
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/some_endpoint", httpContent);

            if ((int)response.StatusCode == 401)
            {
                throw new AuthenticationException($"not authorized");
            }

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var token_obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

            return token_obj["token"]; ;
        }
    }
}
