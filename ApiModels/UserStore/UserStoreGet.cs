using Newtonsoft.Json;
using StarterApi.ApiModels.Store;
using StarterApi.ApiModels.User;

namespace StarterApi.ApiModels.UserStore
{
    public class UserStoreGet
    {
        [JsonProperty(Required = Required.Always)]
        public StoreGet Store { get; set; }


        [JsonProperty(Required = Required.Always)]
        public UserGet User { get; set; } 
    }
}
