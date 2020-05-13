using System;

namespace MiniMdb.Backend.Shared
{
    /// <summary>
    /// Pagination details
    /// </summary>
    public class ApiPagination
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int Count { get; set; }

        public ApiPagination() { }

        public ApiPagination(int page, int pageSize, int count)
        {
            Page = page;
            PageSize = pageSize;
            Count = count;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        }
    }
}
