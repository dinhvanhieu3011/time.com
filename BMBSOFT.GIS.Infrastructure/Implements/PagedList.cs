using BMBSOFT.GIS.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BMBSOFT.GIS.Infrastructure.Implements
{
    public class PagedList<TItem> : IPagedList<TItem>
    {
        public TItem[] Items { get; set; }
        public int PageCount { get; set; }
        public int TotalItemCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public long? CgisId { get; set; }

    }

}
