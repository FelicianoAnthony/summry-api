using StarterApi.ApiModels.Platform;
using StarterApi.Entities;
using StarterApi.Services.BaseService;

namespace StarterApi.Services.Platforms
{
    public interface IPlatformService : IBaseService<Platform, PlatformGet, PlatformPost, PlatformPatch, PlatformQueryParams>
    {
        Task<Platform> FindByName(string name);

    }
}
