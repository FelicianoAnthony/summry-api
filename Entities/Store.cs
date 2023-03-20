using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StarterApi.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StarterApi.Entities
{
    public class Store : BaseTimestamp
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }


        // foreign keys 
        public long PlatformId { get; set; }

        public virtual Platform Platform { get; set; }


        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<UserStore> UserStore { get; set; } 

    }
}
