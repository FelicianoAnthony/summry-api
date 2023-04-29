using Newtonsoft.Json;
using SummryApi.ApiModels.Platform;
using SummryApi.ApiModels.Product;
using System.Text.Json.Serialization;

namespace SummryApi.ApiModels.Store
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
        public List<ProductGet> Products { get; set; }

        public bool ShouldSerializeProducts()
        { 
            return Products != null && Products.Count > 0;
        }

    }
}
