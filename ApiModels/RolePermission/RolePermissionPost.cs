using Newtonsoft.Json;

namespace SummryApi.ApiModels.RolePermission
{
    public class RolePermissionPost
    {

        [JsonProperty(Required = Required.Always)]
        public string Controller { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string Action { get; set; }


        [JsonProperty(Required = Required.Always)]
        public string Role { get; set; }
    }
}
