namespace SummryApi.ApiModels.UserSummryQuery
{
    public class UserSummryQueryPatch : BaseUserSummryQuery
    {
        public float? Price { get; set; }

        public int? MostRecentMinutes { get; set; }

        //   public bool? User... = this would mean one user's query can be switched to another's -- no need
    }
}
