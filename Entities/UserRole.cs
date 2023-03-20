using StarterApi.Entities.Base;

namespace StarterApi.Entities
{
    public class UserRole : BaseTimestamp
    {
        public long Id { get; set; }


        // foreign keys...        
        public long UserId { get; set; }

        public virtual User User { get; set; }


        // foreign keys...        
        public long RoleId { get; set; }

        public virtual Role Role { get; set; }


    }
}
