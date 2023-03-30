using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SummryApi.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SummryApi.Entities
{
    public class Product : BaseTimestamp
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public float Price { get; set; }

        public DateTime? PublishedAt { get; set; }

        public bool Available { get; set; }

        public string Description { get; set; }


        // foreign keys...        
        public long StoreId { get; set; }

        public virtual Store Store { get; set; }
    }
}
