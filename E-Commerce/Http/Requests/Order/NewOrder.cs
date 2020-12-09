using System;
using System.ComponentModel.DataAnnotations;
using E_Commerce.Http.ValidationRules;

namespace E_Commerce.Http.Requests.Order
{
    public class NewOrder
    {
        public string Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [ProductId]
        public Guid[] Products { get; set; }
    }
}