using SummryApi.ApiModels.Platform;
using SummryApi.Entities;

namespace SummryApi.Services.Platforms
{
    public interface IPlatformService
    {
        Platform ConvertToEntity(PlatformPost req);

        Task<bool> Delete(Platform row);

        Task<List<PlatformGet>> GetMany(PlatformQueryParams queryParams);

        Task<PlatformGet> GetOne(long id, PlatformQueryParams queryParams);

        Task<Platform> GetEntity(long id, PlatformQueryParams queryParams);

        Task<PlatformGet> Save(Platform newRow);

        Task<PlatformGet> Update(Platform existingRow, PlatformPatch req);

        Task<Platform> FindByName(string name);

    }
}
