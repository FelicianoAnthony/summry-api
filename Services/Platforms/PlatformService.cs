using StarterApi.ApiModels.Platform;
using StarterApi.ApiModels.Store;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;
using StarterApi.Services.BaseService;

namespace StarterApi.Services.Platforms
{
    public class PlatformService : IBaseService<Platform, PlatformGet, PlatformPost, PlatformPatch, PlatformQueryParams>, IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlatformService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Platform ConvertToEntity(PlatformPost req)
        {
            return new Platform
            {
                Name = req.Name,
                DisplayName = req.DisplayName,
                Description = req.Description
            };
        }

        public async Task<bool> Delete(Platform row)
        {
            _unitOfWork.Platforms.Delete(row);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<List<PlatformGet>> GetMany(PlatformQueryParams? queryParams)
        {
            var entities = await _unitOfWork.Platforms.GetEntities(queryParams);
            return entities.Select(s => TransformOne(s, queryParams)).ToList();
        }

        public async Task<PlatformGet> GetOne(long id, PlatformQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }


        public async Task<Platform> GetEntity(long id, PlatformQueryParams? queryParams)
        {
            Platform platform = await _unitOfWork.Platforms.GetEntity(id, queryParams);
            if (platform == null)
            {
                throw new NotFoundException($"Platform ID '{id}' was not found");
            }
            return platform;
        }


        public async Task<PlatformGet> Save(Platform newRow)
        {
            await _unitOfWork.Platforms.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }


        public async Task<PlatformGet> Update(Platform existingRow, PlatformPatch req)
        {
            existingRow.Name = req.Name ?? existingRow.Name;
            existingRow.DisplayName = req.DisplayName ?? existingRow.DisplayName;
            existingRow.Description = req.Description ?? existingRow.Description;

            _unitOfWork.Platforms.Update(existingRow);
            await _unitOfWork.CompleteAsync();
            return await GetOne(existingRow.Id, null);
        }


        public PlatformGet TransformOne(Platform row, PlatformQueryParams? queryParams)
        {
            queryParams = queryParams ?? new PlatformQueryParams();

            return new PlatformGet
            {
                Id = row.Id,
                Name = row.Name,
                DisplayName = row.DisplayName,
                Description = row.Description,
                Stores = queryParams.ShowStores == true
                    ? row.Stores.Select(s => new StoreGet
                    {
                        Id = s.Id,
                        Url = s.Url,
                    }).ToList()
                    : null,

            };
        }

        // non inherited 
        public async Task<Platform> FindByName(string name)
        {
            var platform = await _unitOfWork.Platforms.FindByName(name);
            if (platform == null)
            {
                throw new NotFoundException($"Platform '{name}' not found.  It must exist before adding a new store");
            }
            return platform;
        }
    }
}
