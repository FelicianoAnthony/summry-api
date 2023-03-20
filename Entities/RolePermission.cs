using StarterApi.Entities.Base;

namespace StarterApi.Entities
{
    public class RolePermission : BaseTimestamp
    {
        public long Id { get; set; }


        // foreign keys...        
        public long RoleId { get; set; }

        public virtual Role Role { get; set; }

        // foreign keys...        
        public long PermissionId { get; set; }

        public virtual Permission Permission { get; set; }

    }
}
