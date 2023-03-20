using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StarterApi.ApiModels.Login
{
    public class LoginRequest
    {
        [EmailAddress]
        [JsonProperty(Required = Required.Always)]
        public string Email { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string Password { get; set; }

    }
}
