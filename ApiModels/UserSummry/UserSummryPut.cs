using StarterApi.ApiModels.Store;
using StarterApi.ApiModels.UserSummryQuery;
using System.ComponentModel.DataAnnotations;

namespace StarterApi.ApiModels.UserSummry
{
    public class UserSummryPut
    {
        [MinLength(1)]
        public List<StorePost> Stores { get; set; }

        [MinLength(1)]
        public List<UserSummryQueryPost> Queries { get; set; }
    }
}
