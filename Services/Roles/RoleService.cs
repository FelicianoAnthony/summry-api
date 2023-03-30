using SummryApi.ApiModels.Permission;
using SummryApi.ApiModels.Role;
using SummryApi.Entities;
using SummryApi.Middlewares.Exceptions;
using SummryApi.Repositories.UnitOfWork;
using SummryApi.Services.BaseService;

namespace SummryApi.Services.Roles
{
    public class RoleService : IBaseService<Role, RoleGet, RolePost, RolePatch, RoleQueryParams>, IRoleService
    {

        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Role ConvertToEntity(RolePost req)
        {
            return new Role
            {
                Name = req.Name,
                Description = req.Description
            };
        }


        public async Task<bool> Delete(Role row)
        {
            _unitOfWork.Roles.Delete(row);
            await _unitOfWork.CompleteAsync();
            return true;
        }


        public async Task<List<RoleGet>> GetMany(RoleQueryParams? queryParams)
        {
            var entities = await _unitOfWork.Roles.GetEntities(queryParams);
            return entities.Select(s => TransformOne(s, queryParams)).ToList();
        }


        public async Task<RoleGet> GetOne(long id, RoleQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }


        public async Task<Role> GetEntity(long id, RoleQueryParams? queryParams)
        {
            Role platform = await _unitOfWork.Roles.GetEntity(id, queryParams);
            if (platform == null)
            {
                throw new NotFoundException($"Role ID '{id}' was not found");
            }
            return platform;
        }


        public async Task<RoleGet> Save(Role newRow)
        {
            await _unitOfWork.Roles.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }


        public async Task<RoleGet> Update(Role existingRow, RolePatch req)
        {
            existingRow.Name = req.Name ?? existingRow.Name;
            existingRow.Description = req.Description ?? existingRow.Description;

            _unitOfWork.Roles.Update(existingRow);
            await _unitOfWork.CompleteAsync();
            return await GetOne(existingRow.Id, null);
        }


        public RoleGet TransformOne(Role row, RoleQueryParams? queryParams)
        {
            queryParams = queryParams ?? new RoleQueryParams();

            return new RoleGet
            {
                Id = row.Id,
                Name = row.Name,
                Description = row.Description,
                Permissions = queryParams.ShowPermissions == true
                    ? row.RolePermissions.Select(p => new PermissionGet
                    {
                        Id = p.Permission.Id,
                        Controller = p.Permission.Controller,
                        Action = p.Permission.Action,
                        Description = p.Permission.Description
                    }).ToList()
                    : null,

            };
        }

        public async Task<Role> FindOneByProps(string name)
        {
            var row = await _unitOfWork.Roles.FindOneByProps(new RoleQueryParams { Name = name });
            if (row == null)
            {
                throw new BadHttpRequestException($"role '{name}' must exist before it can be added");
            }
            return row;
        }
    }
}
