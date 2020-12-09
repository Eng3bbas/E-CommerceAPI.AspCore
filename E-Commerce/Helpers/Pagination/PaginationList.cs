using System.Collections.Generic;

namespace E_Commerce.Helpers.Pagination
{
    public class PaginationList<TEntity> where TEntity : class
    {
        public PaginationMetadata Metadata { get; set; }
        public List<TEntity> Rows { get; set; }
    }
}