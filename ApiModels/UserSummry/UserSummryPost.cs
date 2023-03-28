using Newtonsoft.Json;

namespace StarterApi.ApiModels.UserSummry
{
    public class UserSummryPost
    {
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }
    }
}
