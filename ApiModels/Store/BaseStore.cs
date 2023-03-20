using System.ComponentModel.DataAnnotations;

namespace StarterApi.ApiModels.Store
{
    public class BaseStore
    {
        public virtual string? Name { get; set; } = null!;

        [Url] // data annotation doesnt work if applied in derives class...
        public virtual string? Url { get; set; } = null!;

        public virtual string? Description { get; set; } = null!;
    }
}
