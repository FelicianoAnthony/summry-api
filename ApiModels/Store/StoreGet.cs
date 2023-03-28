using Newtonsoft.Json;
using StarterApi.ApiModels.Platform;
using StarterApi.ApiModels.Product;
using System.Text.Json.Serialization;

namespace StarterApi.ApiModels.Store
{
    public class StoreGet : BaseStore
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string Url { get; set; }


        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PlatformGet Platform { get; set; }


        // foreign keys
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ProductGet> Products { get; set; }

    }
}
