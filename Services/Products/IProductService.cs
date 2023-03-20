using StarterApi.ApiModels.Product;
using StarterApi.Entities;
using StarterApi.Services.BaseService;

namespace StarterApi.Services.Products
{
    public interface IProductService : IBaseService<Product, ProductGet, ProductPost, ProductPatch, ProductQueryParams>
    {
        bool CustomProductMethod();
    }
}
