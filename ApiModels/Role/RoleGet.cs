using Newtonsoft.Json;
using SummryApi.ApiModels.Permission;
using System.Text.Json.Serialization;

namespace SummryApi.ApiModels.Role
{
    public class RoleGet : BaseRole
    {
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Name { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Description { get; set; }


        // foreign keys
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<PermissionGet>? Permissions { get; set; }
    }
}
