using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace StarterApi.ApiModels.Query
{
    public class QueryGet : BaseQuery
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public override string? Producer { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public override string? Bottle { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public override float? Price { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public override int? MostRecentMinutes { get; set; }


        // foreign keys
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Entities.User? User { get; set; }
    }
}
