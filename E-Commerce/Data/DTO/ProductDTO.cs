using System;

namespace E_Commerce.Data.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public CategoryDTO Category { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string CreatedAt { get; set; }
    }
}