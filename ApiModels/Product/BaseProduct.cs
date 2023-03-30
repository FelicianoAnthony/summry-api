namespace SummryApi.ApiModels.Product
{
    public class BaseProduct
    {
        public virtual string? Title { get; set; }

        public virtual float Price { get; set; }

        public virtual DateTime? PublishedAt { get; set; }

        public virtual bool Available { get; set; }

        public virtual string? Description { get; set; }
    }
}
