using System;

namespace E_Commerce.Data.Entities
{
    public class RevokedToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime RevokedAt { get; set; }
    }
}