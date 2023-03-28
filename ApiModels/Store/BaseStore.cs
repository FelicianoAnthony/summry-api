using System.ComponentModel.DataAnnotations;

namespace StarterApi.ApiModels.Store
{
    public class BaseStore
    {
        [Url] // data annotation doesnt work if applied in derives class...
        public virtual string? Url { get; set; } = null!;
    }
}
