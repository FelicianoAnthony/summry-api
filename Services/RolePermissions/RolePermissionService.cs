using StarterApi.ApiModels.RolePermission;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;

namespace StarterApi.Services.RolePermissions
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolePermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public RolePermission ConvertToEntity(Role role, Permission permission)
        {
            return new RolePermission 
            { 
                Role = role,
                Permission = permission
            };
        }


        public async Task<bool> Delete(RolePermission entity)
        {
            _unitOfWork.RolePermissions.Delete(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<RolePermission> GetEntity(long id, RolePermissionQueryParams? queryParams)
        {
            RolePermission row = await _unitOfWork.RolePermissions.GetEntity(id, queryParams);
            if (row == null)
            {
                throw new NotFoundException($"role permission ID '{id}' was not found");
            }
            return row;
        }

        public async Task<List<RolePermissionGet>> GetMany(RolePermissionQueryParams? queryParams)
        {
            var entities = await _unitOfWork.RolePermissions.GetEntities(queryParams);
            return entities.Select(s => TransformOne(s, queryParams)).ToList();
        }

        public async Task<RolePermissionGet> GetOne(long id, RolePermissionQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);

        }

        public async Task<RolePermissionGet> Save(RolePermission newRow)
        {
            await _unitOfWork.RolePermissions.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }

        public RolePermissionGet TransformOne(RolePermission row, RolePermissionQueryParams? queryParams)
        {
            queryParams = queryParams == null ? new RolePermissionQueryParams() : queryParams;
            return new RolePermissionGet
            {
                Id = row.Id,
                Role = row.Role.Name,
                Controller = row.Permission.Controller,
                Action = row.Permission.Action
            };
        }
    }
}
