using Newtonsoft.Json;

namespace SummryApi.ApiModels.Login
{
    public class LoginResponse
    {
        [JsonProperty(Required = Required.Always)]
        public string Token { get; set; }
    }
}
