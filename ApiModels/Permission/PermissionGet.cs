using Newtonsoft.Json;
using StarterApi.ApiModels.Role;
using System.Text.Json.Serialization;

namespace StarterApi.ApiModels.Permission
{
    public class PermissionGet : BasePermission
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public override string? Controller { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Action { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Description { get; set; }

        //// foreign keys
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<RoleGet>? Roles{ get; set; }

    }
}
