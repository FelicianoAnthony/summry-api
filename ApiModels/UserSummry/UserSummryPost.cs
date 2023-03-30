using Newtonsoft.Json;

namespace SummryApi.ApiModels.UserSummry
{
    public class UserSummryPost
    {
        [JsonProperty(Required = Required.Always)]
        public string Title { get; set; }
    }
}
