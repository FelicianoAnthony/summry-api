using SummryApi.Entities.Base;

namespace SummryApi.Entities
{
    public class Role : BaseTimestamp
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

    }
}
