using SummryApi.Entities.Base;

namespace SummryApi.Entities
{
    public class UserSummry : BaseTimestamp
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public bool IsPaused { get; set; }

        // foreign keys...        
        public long UserId { get; set; }

        public virtual User User { get; set; }


        public virtual ICollection<UserSummryStore> UserSummryStores { get; set; }

        public virtual ICollection<UserSummryQuery> UserSummryQueries { get; set; }

    }
}
