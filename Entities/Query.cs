using StarterApi.Entities.Base;

namespace StarterApi.Entities
{
    public class Query : BaseTimestamp
    {
        public long Id { get; set; }
        public string Producer { get; set; }

        public string Bottle { get; set; }

        public float Price { get; set; }

        public int? MostRecentMinutes { get; set; }


        // foreign keys...        
        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}
