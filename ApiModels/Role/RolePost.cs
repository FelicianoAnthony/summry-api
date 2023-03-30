using Newtonsoft.Json;

namespace SummryApi.ApiModels.Role
{
    public class RolePost : BaseRole
    {

        [JsonProperty(Required = Required.Always)]
        public override string? Name { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Description { get; set; }

    }
}
