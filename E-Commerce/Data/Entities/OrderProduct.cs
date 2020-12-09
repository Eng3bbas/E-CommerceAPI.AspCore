using System;

namespace E_Commerce.Data.Entities
{
    public class OrderProduct : BaseEntity
    {
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}