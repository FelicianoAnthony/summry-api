namespace StarterApi.ApiModels.User
{
    public class UserQueryParams : BaseUser
    {
        public bool? ShowQueries { get; set; }

        public bool? ShowStores { get; set; }
    }
}
