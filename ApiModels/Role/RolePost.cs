using Newtonsoft.Json;

namespace StarterApi.ApiModels.Role
{
    public class RolePost : BaseRole
    {

        [JsonProperty(Required = Required.Always)]
        public override string? Name { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Description { get; set; }

    }
}
