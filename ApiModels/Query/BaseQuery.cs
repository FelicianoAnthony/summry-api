namespace StarterApi.ApiModels.Query
{
    public class BaseQuery
    {
        public virtual string? Producer { get; set; }

        public virtual string? Bottle { get; set; }

        public virtual float? Price { get; set; }

        public virtual int? MostRecentMinutes { get; set; }
    }
}
