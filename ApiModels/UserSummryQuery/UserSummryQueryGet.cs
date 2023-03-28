using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace StarterApi.ApiModels.UserSummryQuery
{
    public class UserSummryQueryGet : BaseUserSummryQuery
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public override string? Merchant { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public override string? Product { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public float? Price { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public int? MostRecentMinutes { get; set; }


        // foreign keys
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Entities.User? User { get; set; }
    }
}
