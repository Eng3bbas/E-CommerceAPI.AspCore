using E_Commerce.Data.DTO;

namespace E_Commerce.Http.Responses.Categories
{
    public class SingleResponse : BaseResponse
    {
        public CategoryDTO Category { get; set; }
    }
}