namespace StarterApi.ApiModels.Query
{
    public class QueryQueryParams : BaseQuery
    {
        public bool? ShowUser { get; set; }

        public float? MaxPrice { get; set; }

        public int? MaxRecentMinutes { get; set; }

        public long? UserId { get; set; }
    }
}
