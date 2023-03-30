using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SummryApi.ApiModels.Store
{
    public class StorePost
    {
        [Url]
        [JsonProperty(Required = Required.DisallowNull)]
        public string Url { get; set; }

    }
}
