namespace SummryApi.ApiModels.Permission
{
    public class BasePermission
    {
        public virtual string? Controller { get; set; }

        public virtual string? Action { get; set; }

        public virtual string? Description { get; set; }
    }
}
