using Newtonsoft.Json;

namespace StarterApi.ApiModels.Login
{
    public class LoginResponse
    {
        [JsonProperty(Required = Required.Always)]
        public string Token { get; set; }
    }
}
