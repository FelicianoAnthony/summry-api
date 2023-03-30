namespace SummryApi.ApiModels.Store
{
    public class StoreQueryParams : BaseStore
    {
        public bool? ShowProducts { get; set; }

        public bool? ShowPlatform { get; set; }

        public long? PlatformId { get; set; }

        public string? Platform { get; set; }

    }
}
