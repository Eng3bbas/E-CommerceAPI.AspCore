using E_Commerce.Data.DTO;

namespace E_Commerce.Http.Responses.Orders
{
    public class SingleOrderResponse : BaseResponse
    {
        public SingleOrderDTO Order { get; set; }
    }
}