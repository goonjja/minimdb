using System;

namespace MiniMdb.Backend.Shared
{
    /// <summary>
    /// Pagination details
    /// </summary>
    public class ApiPagination
    {
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages { get;  }
        public int Count { get; }

        public ApiPagination(int page, int pageSize, int count)
        {
            Page = page;
            PageSize = pageSize;
            Count = count;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        }
    }
}
