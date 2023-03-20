using LbAutomationPortalApi.Repositories;
using LinqKit;
using StarterApi.ApiModels.Platform;
using StarterApi.ApiModels.Role;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;
using System.Linq.Expressions;

namespace StarterApi.Repositories.Roles
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly List<string> _permissionsTable = new() { "RolePermissions.Permission" };

        public RoleRepository(StarterApiContext context) : base(context)
        {
        }

        public async Task<List<Role>> GetEntities(RoleQueryParams? queryParams)
        {
            Expression<Func<Role, bool>> predicate = BuildPredicate(queryParams, null);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            IEnumerable<Role> rows = await FindManyWithRelated(predicate, relatedTables);
            return rows.ToList();
        }


        public async Task<Role> GetEntity(long id, RoleQueryParams? queryParams)
        {
            Expression<Func<Role, bool>> predicate = BuildPredicate(queryParams, id);
            List<string> relatedTables = BuildRelatedEntitiesLookup(queryParams);
            Role row = await FindOneWithRelated(predicate, relatedTables);
            return row;
        }


        public async Task<Role> FindOneByProps(RoleQueryParams queryParams)
        {
            Expression<Func<Role, bool>> predicate = BuildPredicate(queryParams, null);
            Role row = await FindOneWithRelated(predicate, new List<string>());
            return row;
        }


        // private methods
        private List<string> BuildRelatedEntitiesLookup(RoleQueryParams? queryParams)
        {
            queryParams = queryParams ?? new RoleQueryParams();
            var relatedTables = new List<string>();
            if (queryParams.ShowPermissions == true)
            {
                relatedTables.AddRange(_permissionsTable);
            }
            return relatedTables;
        }


        private Expression<Func<Role, bool>> BuildPredicate(RoleQueryParams? queryParams, long? id)
        {
            queryParams = queryParams == null ? new RoleQueryParams() : queryParams;

            Expression<Func<Role, bool>> predicate = PredicateBuilder.New<Role>(true);

            if (queryParams.Name != null)
            {
                predicate = predicate.And(r => r.Name.Contains(queryParams.Name));
            }


            if (queryParams.Description != null)
            {
                predicate = predicate.And(r => r.Description.Contains(queryParams.Description));
            }


            if (id != null)
            {
                predicate = predicate.And(r => r.Id == id);
            }

            return predicate;
        }
    }
}
