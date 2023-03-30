using Newtonsoft.Json;

namespace SummryApi.ApiModels.Permission
{
    public class PermissionPost : BasePermission
    {

        [JsonProperty(Required = Required.Always)]
        public override string? Controller { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Action { get; set; }


        [JsonProperty(Required = Required.Always)]
        public override string? Description { get; set; }

    }
}
