﻿using Newtonsoft.Json;
using SummryApi.Entities.Base;

namespace SummryApi.Entities
{
    public class User : BaseTimestamp
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public virtual ICollection<UserSummry> UserSummries { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
