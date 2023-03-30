using Newtonsoft.Json;
using SummryApi.ApiModels.Store;
using System.Text.Json.Serialization;

namespace SummryApi.ApiModels.Product
{
    public class ProductGet : BaseProduct
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Title { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override float Price { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override DateTime? PublishedAt { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override bool Available { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Description { get; set; }


        // foreign keys
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public StoreGet? Store { get; set; }

    }
}
