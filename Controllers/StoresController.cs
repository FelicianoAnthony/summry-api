using Microsoft.AspNetCore.Mvc;
using SummryApi.Entities;
using SummryApi.ApiModels.Store;
using SummryApi.Services.Stores;
using SummryApi.Services.Products;
using SummryApi.ApiModels.Product;

namespace SummryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeSvc;
        private readonly IProductService _productSvc;

        public StoresController(IStoreService storeSvc, IProductService productSvc)
        {
            _storeSvc = storeSvc;
            _productSvc = productSvc;
        }


        [HttpGet]
        public async Task<ActionResult<List<StoreGet>>> GetStores([FromQuery] StoreQueryParams queryParams)
        {
            return await _storeSvc.GetMany(queryParams);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<StoreGet>> GetStore(long id, [FromQuery] StoreQueryParams queryParams)
        {
            return await _storeSvc.GetOne(id, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<StoreGet>> PostStore(StorePost store)
        {
            Platform platform = await _storeSvc.CheckUrlScrapability(store.Url);
            if (platform == null)
            {
                throw new Exception($"url doesnt match any platforms in DB for which scraping logic has been developed for");
            }
            Store newRow = _storeSvc.ConvertToEntity(store, platform);
            return await _storeSvc.Save(newRow);
        }


        [HttpPatch("{id}")]
        public async Task<StoreGet> PutStore(long id, [FromBody] StorePatch req)
        {
            Store existingRow = await _storeSvc.GetEntity(id, null);
            return await _storeSvc.Update(existingRow, req);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            Store store = await _storeSvc.GetEntity(id, new StoreQueryParams { ShowProducts = true });
            bool result = await _storeSvc.Delete(store);
            return NoContent();
        }

        // PRODUCTS 
        [HttpGet("{storeId}/Products")]
        public async Task<List<ProductGet>> GetStoreProducts(long storeId, [FromQuery] ProductQueryParams queryParams)
        {
            await _storeSvc.GetEntity(storeId, null);
            queryParams.StoreId = storeId;
            return await _productSvc.GetMany(queryParams);
        }


        [HttpPost("{id}/Products")]
        public async Task<ProductGet> AddStoreProduct(long id, [FromBody] ProductPost req)
        {
            Product newProduct = _productSvc.ConvertToEntity(req);
            newProduct.Store = await _storeSvc.GetEntity(id, null);
            return await _productSvc.Save(newProduct);
        }


    }
}
