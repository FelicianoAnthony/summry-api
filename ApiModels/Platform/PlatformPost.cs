using Newtonsoft.Json;

namespace SummryApi.ApiModels.Platform
{
    public class PlatformPost
    {
        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string DisplayName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; }
    }
}
