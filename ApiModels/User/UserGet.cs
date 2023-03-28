using Newtonsoft.Json;
using StarterApi.ApiModels.UserSummry;
using System.Text.Json.Serialization;

namespace StarterApi.ApiModels.User
{
    public class UserGet     
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string FirstName { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string LastName { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string Email { get; set; }

        // foreign keys
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<UserSummryGet> Summries { get; set; }


    }
}
