using System.Collections.Generic;
using E_Commerce.Data.DTO;

namespace E_Commerce.Http.Responses.Categories
{
    public class ListResponse : BaseResponse
    {
        public List<CategoryDTO> Categories { get; set; }
    }
}