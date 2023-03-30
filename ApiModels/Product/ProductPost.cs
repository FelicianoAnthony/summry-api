using Newtonsoft.Json;

namespace SummryApi.ApiModels.Product
{
    public class ProductPost : BaseProduct
    {
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
    }
}
