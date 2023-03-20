using Microsoft.AspNetCore.Mvc;
using StarterApi.Entities;
using StarterApi.ApiModels.Store;
using StarterApi.Services.Stores;
using StarterApi.Services.Platforms;
using StarterApi.ApiModels.Platform;

namespace StarterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformSvc;
        private readonly IStoreService _storeSvc;

        public PlatformsController(IPlatformService platformSvc, IStoreService storeSvc)
        {
            _platformSvc = platformSvc;
            _storeSvc = storeSvc;
        }


        [HttpGet]
        public async Task<ActionResult<List<PlatformGet>>> GetMany([FromQuery] PlatformQueryParams queryParams)
        {
            return await _platformSvc.GetMany(queryParams);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformGet>> GetOne(long id, [FromQuery] PlatformQueryParams queryParams)
        {
            return await _platformSvc.GetOne(id, queryParams);
        }


        [HttpPost]
        public async Task<ActionResult<PlatformGet>> AddOne([FromBody] PlatformPost platform)
        {
            Platform newRow = _platformSvc.ConvertToEntity(platform);
            return await _platformSvc.Save(newRow);
        }


        [HttpPatch("{id}")]
        public async Task<PlatformGet> ModifyOne(long id, [FromBody] PlatformPatch req)
        {
            Platform existingRow = await _platformSvc.GetEntity(id, null);
            return await _platformSvc.Update(existingRow, req);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            Platform store = await _platformSvc.GetEntity(id, null);
            bool result = await _platformSvc.Delete(store);
            return NoContent();
        }

        // STORES 
        [HttpGet("{platformId}/stores")]
        public async Task<List<StoreGet>> GetPlatformStores(long platformId, [FromQuery] StoreQueryParams queryParams)
        {
            await _storeSvc.GetEntity(platformId, null);
            queryParams.PlatformId = platformId;
            return await _storeSvc.GetMany(queryParams);
        }


        [HttpPost("{id}/stores")]
        public async Task<StoreGet> AddStoreToPlatform(long id, [FromBody] StorePost req)
        {
            Store newStore = _storeSvc.ConvertToEntity(req);
            newStore.Platform = await _platformSvc.GetEntity(id, null);
            return await _storeSvc.Save(newStore);
        }
    }
}
