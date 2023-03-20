using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StarterApi.ApiModels.User
{
    public class UserPost
    {
        [JsonProperty(Required = Required.Always)]
        public string FirstName { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string LastName { get; set; }

        [EmailAddress]
        [JsonProperty(Required = Required.Always)]
        public string Email { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Password { get; set; }

    }
}
