using SummryApi.ApiModels.Platform;
using SummryApi.ApiModels.Product;
using SummryApi.ApiModels.Store;
using SummryApi.Entities;
using SummryApi.Middlewares.Exceptions;
using SummryApi.Repositories.UnitOfWork;
using SummryApi.Services.BaseService;

namespace SummryApi.Services.Products
{
    public class ProductService : IBaseService<Product, ProductGet, ProductPost, ProductPatch, ProductQueryParams>, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Product ConvertToEntity(ProductPost postRequest)
        {
            return new Product 
            { 
                Title = postRequest.Title,
                Price = postRequest.Price,
                PublishedAt = postRequest.PublishedAt,
                Available = postRequest.Available,
                Description = postRequest.Description
            };
        }

        public bool CustomProductMethod()
        {
            throw new NotImplementedException();
        }


        public async Task<bool> Delete(Product entity)
        {
            _unitOfWork.Products.Delete(entity);
            await _unitOfWork.CompleteAsync();
            return true;
        }


        public async Task<Product> GetEntity(long id, ProductQueryParams? queryParams)
        {
            Product product = await _unitOfWork.Products.GetEntity(id, queryParams);
            if (product == null)
            {
                throw new NotFoundException($"product ID '{id}' was not found");
            }
            return product;
        }


        public async Task<List<ProductGet>> GetMany(ProductQueryParams? queryParams)
        {
            var storeEntities = await _unitOfWork.Products.GetEntities(queryParams);
            return storeEntities.Select(s => TransformOne(s, queryParams)).ToList();
        }


        public async Task<ProductGet> GetOne(long id, ProductQueryParams? queryParams)
        {
            return TransformOne(await GetEntity(id, queryParams), queryParams);
        }


        public async Task<ProductGet> Save(Product newRow)
        {
            await _unitOfWork.Products.Add(newRow);
            await _unitOfWork.CompleteAsync();

            return TransformOne(await GetEntity(newRow.Id, null), null);
        }


        public ProductGet TransformOne(Product row, ProductQueryParams? queryParams)
        {
            queryParams = queryParams == null ? new ProductQueryParams() : queryParams;

            return new ProductGet
            {
                Id = row.Id,
                Title = row.Title,
                Price = row.Price,
                PublishedAt = row.PublishedAt,
                Available = row.Available,
                Description = row.Description,
                Store = queryParams.ShowStore == true 
                ? new StoreGet
                    {
                        Id = row.Store.Id,
                        Url = row.Store.Url,
                        Platform = queryParams.ShowPlatform == true ? new PlatformGet
                        {
                            Id = row.Store.Platform.Id,
                            Name = row.Store.Platform.Name,
                            DisplayName = row.Store.Platform.DisplayName,
                            Description = row.Store.Platform.Description
                        }
                        : null,
                } 
                : null  
            };
        }


        public Task<ProductGet> Update(Product existingRow, ProductPatch patchRequest)
        {
            // products are only able to be DELET-ed or POST-ed...
            throw new NotImplementedException();
        }
    }
}
