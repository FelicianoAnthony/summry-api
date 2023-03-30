namespace SummryApi.ApiModels.UserSummryQuery
{
    public class UserSummryQueryFilters : BaseUserSummryQuery
    {
        public bool? ShowUser { get; set; }

        public float? MaxPrice { get; set; }

        public int? MaxRecentMinutes { get; set; }

        public long? UserId { get; set; }


        // fix this...
        public float? Price { get; set; }

        public int? MostRecentMinutes { get; set; }
    }
}
