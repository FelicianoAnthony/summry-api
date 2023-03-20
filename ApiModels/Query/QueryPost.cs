using Newtonsoft.Json;

namespace StarterApi.ApiModels.Query
{
    public class QueryPost : BaseQuery
    {
        [JsonProperty(Required = Required.AllowNull)]
        public override string? Producer { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public override string? Bottle { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public new float Price { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public new int MostRecentMinutes { get; set; }
    }
}
