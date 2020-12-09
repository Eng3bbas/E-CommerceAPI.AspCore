using E_Commerce.Data.DTO;

namespace E_Commerce.Http.Responses.Products
{
    public class SingleProductResponse : BaseResponse
    {
        public ProductDTO Product { get; set; }
    }
}