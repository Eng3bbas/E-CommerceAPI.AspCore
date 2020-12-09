using System;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Data.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public  Guid Id { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
        }
    }
}