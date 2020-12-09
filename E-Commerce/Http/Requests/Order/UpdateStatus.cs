using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Http.Requests.Order
{
    public class UpdateStatus
    {
        [Required]
        public Data.Entities.Order.Statuses Status { get; set; }
    }
}