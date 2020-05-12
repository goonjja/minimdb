using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMdb.Backend.Shared
{
    public class DataPage<T>
    {
        public IEnumerable<T> Items { get; }
        public int Count { get; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }

        public DataPage(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            Count = count;
            Page = pageIndex;
            PageSize = pageSize;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            Items = items;
        }

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;

        public static async Task<DataPage<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new DataPage<T>(items, count, pageIndex, pageSize);
        }
    }
}
