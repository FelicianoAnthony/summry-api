using SummryApi.ApiModels.Platform;
using SummryApi.Entities;
using SummryApi.Services.BaseService;

namespace SummryApi.Services.Platforms
{
    public interface IPlatformService : IBaseService<Platform, PlatformGet, PlatformPost, PlatformPatch, PlatformQueryParams>
    {
        Task<Platform> FindByName(string name);

    }
}
