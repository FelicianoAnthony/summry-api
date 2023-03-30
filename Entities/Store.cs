using SummryApi.Entities.Base;

namespace SummryApi.Entities
{
    public class Store : BaseTimestamp
    {
        public long Id { get; set; }

        public string Url { get; set; }


        // foreign keys 
        public long PlatformId { get; set; }

        public virtual Platform Platform { get; set; }


        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<UserSummryStore> UserSummryStore { get; set; } 

    }
}
