using SummryApi.Entities.Base;

namespace SummryApi.Entities
{
    public class Permission : BaseTimestamp
    {
        public long Id { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

    }
}
