namespace StarterApi.ApiModels.Product
{
    public class ProductQueryParams
    {
        public string? Title { get; set; }

        public float? Price { get; set; }

        public DateTime? PublishedAtLaterThan { get; set; }

        public bool? Available { get; set; }

        public string? Description { get; set; }

        public bool? ShowStore { get; set; }

        public bool? ShowPlatform { get; set; }

        public long? StoreId { get; set; }
    }
}
