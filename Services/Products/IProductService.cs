using SummryApi.ApiModels.Product;
using SummryApi.Entities;
using SummryApi.Services.BaseService;

namespace SummryApi.Services.Products
{
    public interface IProductService : IBaseService<Product, ProductGet, ProductPost, ProductPatch, ProductQueryParams>
    {
        bool CustomProductMethod();
    }
}
