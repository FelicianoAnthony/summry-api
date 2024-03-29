﻿using System.ComponentModel.DataAnnotations;

namespace SummryApi.ApiModels.User
{
    public class BaseUser
    {
        public virtual string? FirstName { get; set; }

        public virtual string? LastName { get;set;}

        [EmailAddress]
        public virtual string? Email { get;set; }

    }
}
