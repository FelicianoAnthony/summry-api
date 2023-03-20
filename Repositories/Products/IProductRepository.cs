using StarterApi.ApiModels.Product;
using StarterApi.Entities;
using StarterApi.Repositories.Generic;

namespace StarterApi.Repositories.Products
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetEntities(ProductQueryParams? queryParams);

        Task<Product> GetEntity(long id, ProductQueryParams? queryParams);
    }
}
