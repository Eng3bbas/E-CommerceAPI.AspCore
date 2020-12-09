using E_Commerce.Data.DTO;
using E_Commerce.Helpers.Pagination;

namespace E_Commerce.Http.Responses.Products
{
    public class ProductsListResponse : BaseResponse
    {
        public PaginationList<ProductDTO> Data { get; set; }
    }
}