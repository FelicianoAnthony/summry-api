using AutoMapper;
using SummryApi.ApiModels.Platform;
using SummryApi.Entities;
using SummryApi.Middlewares.Exceptions;
using SummryApi.Repositories.UnitOfWork;

namespace SummryApi.Services.Platforms
{
    public class PlatformService : IPlatformService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlatformService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            List<Platform> entities = await _unitOfWork.Platforms.GetEntities(queryParams);
            return _mapper.Map<List<PlatformGet>>(entities);
        }

        public async Task<PlatformGet> GetOne(long id, PlatformQueryParams? queryParams)
        {
            var entity = await GetEntity(id, queryParams);
            return _mapper.Map<PlatformGet>(entity);
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

            Platform entity = await GetEntity(newRow.Id, null);
            return _mapper.Map<PlatformGet>(entity);
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
