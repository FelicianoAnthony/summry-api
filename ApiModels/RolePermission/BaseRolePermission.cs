namespace SummryApi.ApiModels.RolePermission
{
    public class BaseRolePermission
    {
        public virtual string? Controller { get; set; }

        public virtual string? Action { get; set; }

        public virtual string? Role { get; set; }
    }
}
