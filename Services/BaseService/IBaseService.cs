namespace StarterApi.Services.BaseService
{
    public interface IBaseService<TYPE, GET, POST, PATCH, QUERY> 
        where TYPE : class
        where GET : class 
        where POST : class 
        where PATCH : class
        where QUERY : class
    {

        TYPE ConvertToEntity(POST postRequest);

        Task<bool> Delete(TYPE entity);

        Task<TYPE> GetEntity(long id, QUERY? queryParams);

        Task<List<GET>> GetMany(QUERY? queryParams);

        Task<GET> GetOne(long id, QUERY? queryParams);

        Task<GET> Update(TYPE existingRow, PATCH patchRequest);

        Task<GET> Save(TYPE newRow);

        GET TransformOne(TYPE row, QUERY? queryParams);

    }
}
