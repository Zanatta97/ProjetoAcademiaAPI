using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoAcademiaAPI.Application.Common
{
    public sealed record PagedResponse<T>
    {
        public IEnumerable<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public PagedResponse(IEnumerable<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public PagedResponse<TResult> Map<TResult>(Func<T, TResult> map) =>
            new([.. Items.Select(map)], Page, PageSize, TotalCount);

    }
}
