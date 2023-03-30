using SummryApi.Entities.Base;

namespace SummryApi.Entities
{
    public class UserSummryStore : BaseTimestamp
    {
        public long Id { get; set; }

        public bool IsPaused { get; set; }


        // foreign keys...        
        public long UserSummryId { get; set; }

        public virtual UserSummry UserSummry { get; set; }


        public long StoreId { get; set; }

        public virtual Store Store { get; set; }
    }
}
