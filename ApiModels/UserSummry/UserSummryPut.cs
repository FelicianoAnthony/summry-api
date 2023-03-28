using StarterApi.ApiModels.Store;
using StarterApi.ApiModels.UserSummryQuery;

namespace StarterApi.ApiModels.UserSummry
{
    public class UserSummryPut
    {
        public List<StorePost> Stores { get; set; }

        public List<UserSummryQueryPost> Queries { get; set; }
    }
}
