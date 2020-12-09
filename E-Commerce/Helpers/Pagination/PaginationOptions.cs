namespace E_Commerce.Helpers.Pagination
{
    public class PaginationOptions
    {
        public int PerPage { get; set; } = 10;
        public int Page { get; set; } 

        public PaginationOptions()
        {
            if (Page <= 0)
            {
                Page = 1;
            }
        }
    }
}