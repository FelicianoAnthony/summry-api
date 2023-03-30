using SummryApi.ApiModels.Store;
using SummryApi.ApiModels.UserSummryQuery;
using System.Text.Json.Serialization;

namespace SummryApi.ApiModels.UserSummry
{
    public class UserSummryGet
    {
        public UserSummryGet()
        {
            Stores = new List<StoreGet>();
            Queries = new List<UserSummryQueryGet>();
        }
        public long Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }


        // foreign key
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<StoreGet> Stores { get; set; }


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<UserSummryQueryGet> Queries { get; set; }
    }
}
