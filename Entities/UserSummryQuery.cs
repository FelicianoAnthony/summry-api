using StarterApi.Entities.Base;

namespace StarterApi.Entities
{
    public class UserSummryQuery : BaseTimestamp
    {
        public long Id { get; set; }

        public string Merchant { get; set; }

        public string Product { get; set; }

        public float? Price { get; set; }

        public int? MostRecentMinutes { get; set; }

        public bool IsPaused { get; set; }



        // foreign keys...        
        public long UserSummryId { get; set; }

        public virtual UserSummry UserSummry { get; set; }
    }
}
