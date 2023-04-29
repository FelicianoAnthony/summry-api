using Newtonsoft.Json;
using SummryApi.ApiModels.Platform;
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
        // show null since change in Program.cs in this commit...
        //[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public List<ProductGet> Products { get; set; }

    }
}
