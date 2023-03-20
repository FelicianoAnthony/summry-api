using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.ApiModels.Permission;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.Permissions
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public readonly List<string> _roleTable = new() { "RolePermissions.Role" };
        public PermissionRepository(StarterApiContext context) : base(context)
        {
        }

        public async Task<List<Permission>> GetEntities(PermissionQueryParams? queryParams)
        {
            Expression<Func<Permission, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<Permission> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }


        public async Task<Permission> GetEntity(long id, PermissionQueryParams? queryParams)
        {
            Expression<Func<Permission, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            Permission row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }


        public async Task<Permission> FindOneByProps(PermissionQueryParams queryParams)
        {
            Expression<Func<Permission, bool>> predicate = BuildPredicate(queryParams, null);
            Permission row = await FindOneWithRelated(predicate, new List<string>());
            return row;
        }


        // private methods
        private List<string> BuildRelatedEntitiesLookup(PermissionQueryParams? queryParams)
        {
            queryParams = queryParams ?? new PermissionQueryParams();
            var relatedTables = new List<string>();
            if (queryParams.ShowRole == true)
            {
                relatedTables.AddRange(_roleTable);
            }
            return relatedTables;
        }


        private Expression<Func<Permission, bool>> BuildPredicate(PermissionQueryParams? queryParams, long? id)
        {
            queryParams = queryParams == null ? new PermissionQueryParams() : queryParams;

            Expression<Func<Permission, bool>> predicate = PredicateBuilder.New<Permission>(true);

            if (queryParams.Controller != null)
            {
                predicate = predicate.And(p => p.Controller.Contains(queryParams.Controller));
            }

            if (queryParams.Action != null)
            {
                predicate = predicate.And(p => p.Action.Contains(queryParams.Action));
            }

            if (queryParams.Description != null)
            {
                predicate = predicate.And(p => p.Description.Contains(queryParams.Description));
            }


            if (id != null)
            {
                predicate = predicate.And(p => p.Id == id);
            }

            return predicate;
        }

    }
}
