using System.ComponentModel.DataAnnotations;
using E_Commerce.Helpers.Pagination;

namespace E_Commerce.Http.Requests.Order
{
    public class FilterOrders
    {
        public enum OrderingByCreatedAtTypes
        {
            Desc,
            Asc
        }
        public string Address { get; set; }
        [Phone]
        public string Phone { get; set; }
        public Data.Entities.Order.Statuses OrderStatus { get; set; } = Data.Entities.Order.Statuses.Pending;
        public PaginationOptions PaginationOptions { get; set; } = new PaginationOptions();
        public OrderingByCreatedAtTypes OrderingBy { get; set; }
    }
}