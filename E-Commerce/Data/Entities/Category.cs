using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Data.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}