using SummryApi.ApiModels.Platform;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.Platforms
{
    public interface IPlatformRepository : IGenericRepository<Platform>
    {
        Task<List<Platform>> GetEntities(PlatformQueryParams? queryParams);

        Task<Platform> GetEntity(long id, PlatformQueryParams? queryParams);

        Task<Platform> FindByName(string name);
    }
}
