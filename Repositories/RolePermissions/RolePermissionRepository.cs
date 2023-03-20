using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.ApiModels.RolePermission;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.RolePermissions
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        private readonly List<string> _defaultTables = new() { "Role", "Permission" };
        
        public RolePermissionRepository(StarterApiContext context) : base(context)
        {
        }

        public async Task<List<RolePermission>> GetEntities(RolePermissionQueryParams? queryParams)
        {
            Expression<Func<RolePermission, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<RolePermission> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }


        public async Task<RolePermission> GetEntity(long id, RolePermissionQueryParams? queryParams)
        {
            Expression<Func<RolePermission, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            RolePermission row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }


        public async Task<RolePermission> FindOneByProps(RolePermissionQueryParams queryParams)
        {
            Expression<Func<RolePermission, bool>> predicate = BuildPredicate(queryParams, null);
            RolePermission row = await FindOneWithRelated(predicate, new List<string>());
            return row;
        }


        // private methods
        private List<string> BuildRelatedEntitiesLookup(RolePermissionQueryParams? queryParams)
        {
            queryParams = queryParams ?? new RolePermissionQueryParams();
            List<string> relatedTables = _defaultTables;
            return relatedTables;
        }


        private Expression<Func<RolePermission, bool>> BuildPredicate(RolePermissionQueryParams? queryParams, long? id)
        {
            queryParams = queryParams == null ? new RolePermissionQueryParams() : queryParams;

            Expression<Func<RolePermission, bool>> predicate = PredicateBuilder.New<RolePermission>(true);

            if (queryParams.Controller != null)
            {
                predicate = predicate.And(p => p.Permission.Controller.Contains(queryParams.Controller));
            }

            if (queryParams.Action != null)
            {
                predicate = predicate.And(p => p.Permission.Action.Contains(queryParams.Action));
            }

            if (queryParams.Role != null)
            {
                predicate = predicate.And(p => p.Role.Name.Contains(queryParams.Role));
            }


            if (id != null)
            {
                predicate = predicate.And(p => p.Id == id);
            }

            return predicate;
        }
    }
}
