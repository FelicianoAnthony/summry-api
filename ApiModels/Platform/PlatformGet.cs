using Newtonsoft.Json;
using SummryApi.ApiModels.Store;
using System.Text.Json.Serialization;

namespace SummryApi.ApiModels.Platform
{
    public class PlatformGet
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string DisplayName { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; }

        // foreign key
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<StoreGet>? Stores { get; set; }

    }
}
