using System;
using System.Collections.Generic;
using AutoMapper.Configuration.Conventions;
using E_Commerce.Data.Entities;

namespace E_Commerce.Data.DTO
{
    public class SingleOrderDTO
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        
        public Order.Statuses Status { get; set; } = Order.Statuses.Pending;
        public DateTime CreatedAt { get; set; }
        public UserDTO User { get; set; }
        public ICollection<ProductDTO> Products { get; set; }
    }
}