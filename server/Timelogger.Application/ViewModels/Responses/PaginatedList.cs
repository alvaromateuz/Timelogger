using System;
using System.Collections.Generic;

namespace Timelogger.Application.ViewModels.Responses
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public List<T> Items { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public bool HasPreviousPage
        {
            get{ return (PageIndex > 1);}
        }

        public bool HasNextPage
        {
            get{ return (PageIndex < TotalPages);}
        }
    }
}
