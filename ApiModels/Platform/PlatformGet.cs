using Newtonsoft.Json;
using SummryApi.ApiModels.Store;

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
        public List<StoreGet> Stores { get; set; }


        public bool ShouldSerializeStores()
        {
            return Stores != null;
        }

    }
}
