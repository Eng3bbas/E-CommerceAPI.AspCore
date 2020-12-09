using System;
using System.Collections.Generic;

namespace E_Commerce.Data.Entities
{
    public class Order : BaseEntity
    {
        public enum Statuses
        {
            Pending,
            Deleted,
            Completed,
            Approved
        }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Statuses Status { get; set; } = Statuses.Pending;
        public virtual ICollection<OrderProduct> Products { get; set; } = new HashSet<OrderProduct>();
    }
}