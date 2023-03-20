using Newtonsoft.Json;

namespace StarterApi.ApiModels.RolePermission
{
    public class RolePermissionGet : BaseRolePermission
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Controller { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Action { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Role { get; set; }
    }
}
