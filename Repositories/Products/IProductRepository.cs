using SummryApi.ApiModels.Product;
using SummryApi.Entities;
using SummryApi.Repositories.Generic;

namespace SummryApi.Repositories.Products
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetEntities(ProductQueryParams? queryParams);

        Task<Product> GetEntity(long id, ProductQueryParams? queryParams);
    }
}
