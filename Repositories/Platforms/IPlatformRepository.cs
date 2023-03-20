using StarterApi.ApiModels.Platform;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.Platforms
{
    public interface IPlatformRepository : IGenericRepository<Platform>
    {
        Task<List<Platform>> GetEntities(PlatformQueryParams? queryParams);

        Task<Platform> GetEntity(long id, PlatformQueryParams? queryParams);

        Task<Platform> FindByName(string name);
    }
}
