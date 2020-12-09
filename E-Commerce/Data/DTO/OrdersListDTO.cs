using System;
using System.Collections.Generic;
using E_Commerce.Data.Entities;

namespace E_Commerce.Data.DTO
{
    public class OrdersListDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Order.Statuses Status { get; set; } = Order.Statuses.Pending;
        public DateTime CreatedAt { get; set; }
        public int ProductsCount { get; set; }
    }
}