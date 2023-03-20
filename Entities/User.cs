using Newtonsoft.Json;
using StarterApi.Entities.Base;

namespace StarterApi.Entities
{
    public class User : BaseTimestamp
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public virtual ICollection<Query> Queries { get; set; }

        public virtual ICollection<UserStore> UserStore { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
