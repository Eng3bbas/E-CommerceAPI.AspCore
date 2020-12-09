using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Configuration.Conventions;
using E_Commerce.Data.DTO;

namespace E_Commerce.Data.Entities
{
    
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public virtual ICollection<OrderProduct> Orders { get; set; } = new HashSet<OrderProduct>();
    }
}