using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StarterApi.ApiModels.Store
{
    public class StorePost
    {
        [JsonProperty(Required = Required.DisallowNull)]
        public string Name { get; set; }

        [Url]
        [JsonProperty(Required = Required.DisallowNull)]
        public string Url { get; set; }


        [JsonProperty(Required = Required.DisallowNull)]
        public string Description { get; set; }


        // can add a store at both POST /Platforms/<id>/store & POST/Stores
        [JsonProperty(Required = Required.AllowNull)]
        public string? Platform { get; set; }
    }
}
