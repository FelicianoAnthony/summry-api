using StarterApi.Entities.Base;

namespace StarterApi.Entities
{
    public class Platform : BaseTimestamp
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        // foreign keys
        public virtual ICollection<Store> Stores { get; set; }
    }
}
