using Newtonsoft.Json;

namespace SummryApi.ApiModels.UserSummryQuery
{
    public class UserSummryQueryPost : BaseUserSummryQuery
    {
        [JsonProperty(Required = Required.AllowNull)]
        public override string Merchant { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public override string Product { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public float? Price { get; set; }


        [JsonProperty(Required = Required.AllowNull)]
        public int? MostRecentMinutes { get; set; }
    }
}
