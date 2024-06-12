using BASE.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BASE.Infrastructure.Implements
{
    public abstract class BasePagedList<TItem> : IPagedList<TItem>
    {
        private const string _PAGE_NUMBER_BELOW_1 = "Page number cannot be below 1.";
        private const string _PAGE_SIZE_LESS_THAN_1 = "Page size cannot be less than 1.";
        /// <summary>
        /// This constructor is meant to support ToPagedListAsync extension methods, and should only be exposed as <c>internal</c>.
        /// </summary>
        internal BasePagedList()
        {
        }
        protected BasePagedList(IPagedList<TItem> list)
        {
            PageCount = list.PageCount;
            TotalItemCount = list.TotalItemCount;
            PageIndex = list.PageIndex;
            PageSize = list.PageSize;
            HasPreviousPage = list.HasPreviousPage;
            HasNextPage = list.HasNextPage;
            Items = list.Items;
            CgisId = list.CgisId;
        }
        protected BasePagedList(
            IQueryable<TItem> queryable,
            int pageIndex,
            int pageSize,
            bool nopaging = false)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(pageIndex),
                    actualValue: PageIndex,
                    message: _PAGE_NUMBER_BELOW_1
                );
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(pageSize),
                    actualValue: pageSize,
                    message: _PAGE_SIZE_LESS_THAN_1
                );
            }

            TotalItemCount = queryable.Count();
            PageSize = pageSize;
            PageCount =
                TotalItemCount > 0
                    ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
                    : 0;
            PageIndex = pageIndex > PageCount ? PageCount : pageIndex;
            HasPreviousPage = PageIndex > 1;
            HasNextPage = PageIndex < PageCount;
            Items = nopaging
                ? queryable.ToArray()
                : queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();
        }
        protected BasePagedList(IEnumerable<TItem> items, int pageIndex, int pageSize, int totalCount)
        {
            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(pageIndex),
                    actualValue: pageIndex,
                    message: _PAGE_NUMBER_BELOW_1
                );
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(pageSize),
                    actualValue: pageSize,
                    message: _PAGE_SIZE_LESS_THAN_1
                );
            }
            TotalItemCount = totalCount;
            PageSize = pageSize;
            PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;
            PageIndex = pageIndex > PageCount ? PageCount : pageIndex;
            HasPreviousPage = PageIndex > 1;
            HasNextPage = PageIndex < PageCount;
            Items = items?.ToArray() ?? new TItem[0];
        }
        public TItem[] Items { get; set; }

        public int PageCount { get; set; }

        public int TotalItemCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get;  set; }//protected
        public long? CgisId { get; set; }
    }
}
