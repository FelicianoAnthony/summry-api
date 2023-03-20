using Newtonsoft.Json;

namespace StarterApi.ApiModels.Platform
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
