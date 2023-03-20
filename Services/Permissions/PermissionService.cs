using StarterApi.ApiModels.Permission;
using StarterApi.ApiModels.Role;
using StarterApi.Entities;
using StarterApi.Middlewares.Exceptions;
using StarterApi.Repositories.UnitOfWork;
using StarterApi.Services.BaseService;

namespace StarterApi.Services.Permissions
{
    public class PermissionService : IBaseService<Permission, PermissionGet, PermissionPost, PermissionPatch, PermissionQueryParams>, IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Permission ConvertToEntity(PermissionPost req)
        {
            return new Permission
            {
                Controller = req.Controller,
                Action = req.Action,
                Description = req.Description
            };
        }


        public async Task<bool> Delete(Permission row)
        {
            _unitOfWork.Permissions.Delete(row);
            await _unitOfWork.CompleteAsync();
            return true;
        }


        public async Task<List<PermissionGet>> GetMany(PermissionQueryParams? queryParams)
        {
            var entities = await _unitOfWork.Permissions.GetEntities(queryParams);
            return entities.Select(s => TransformOne(s, queryParams)).ToList();
        }


        public async Task<PermissionGet> GetOne(long id, PermissionQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }


        public async Task<Permission> GetEntity(long id, PermissionQueryParams? queryParams)
        {
            Permission platform = await _unitOfWork.Permissions.GetEntity(id, queryParams);
            if (platform == null)
            {
                throw new NotFoundException($"Permission ID '{id}' was not found");
            }
            return platform;
        }


        public async Task<PermissionGet> Save(Permission newRow)
        {
            await _unitOfWork.Permissions.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }


        public async Task<PermissionGet> Update(Permission existingRow, PermissionPatch req)
        {
            existingRow.Controller = req.Controller ?? existingRow.Controller;
            existingRow.Action = req.Action ?? existingRow.Action;
            existingRow.Description = req.Description ?? existingRow.Description;

            _unitOfWork.Permissions.Update(existingRow);
            await _unitOfWork.CompleteAsync();
            return await GetOne(existingRow.Id, null);
        }


        public PermissionGet TransformOne(Permission row, PermissionQueryParams? queryParams)
        {
            queryParams = queryParams ?? new PermissionQueryParams();

            return new PermissionGet
            {
                Id = row.Id,
                Controller = row.Controller,
                Action = row.Action,
                Description = row.Description,
                Roles= queryParams.ShowRole == true
                    ? row.RolePermissions.Select(p => new RoleGet
                    {
                        Id = p.Role.Id,
                        Name = p.Role.Name,
                        Description = p.Role.Description
                    }).ToList()
                    : null,

            };
        }

        public async Task<Permission> FindOneByProps(string controller, string action)
        {
            var row = await _unitOfWork.Permissions.FindOneByProps(new PermissionQueryParams { Controller = controller, Action = action});
            if (row == null)
            {
                throw new BadHttpRequestException($"permission with controller '{controller}' and action '{action}' must exist before it can be added");
            }
            return row;
        }
    }
}
