using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StarterApi.ApiModels.Store
{
    public class StorePost
    {
        [Url]
        [JsonProperty(Required = Required.DisallowNull)]
        public string Url { get; set; }

    }
}
