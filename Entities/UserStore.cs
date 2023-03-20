using StarterApi.Entities.Base;

namespace StarterApi.Entities
{
    public class UserStore : BaseTimestamp
    {
        public long Id { get; set; }


        // foreign keys...        
        public long UserId { get; set; }

        public virtual User User { get; set; }


        // foreign keys...        
        public long StoreId { get; set; }

        public virtual Store Store { get; set; }
    }
}
