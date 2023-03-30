using SummryApi.ApiModels.Store;
using SummryApi.ApiModels.UserSummryQuery;
using System.ComponentModel.DataAnnotations;

namespace SummryApi.ApiModels.UserSummry
{
    public class UserSummryPut
    {
        [MinLength(1)]
        public List<StorePost> Stores { get; set; }

        [MinLength(1)]
        public List<UserSummryQueryPost> Queries { get; set; }
    }
}
