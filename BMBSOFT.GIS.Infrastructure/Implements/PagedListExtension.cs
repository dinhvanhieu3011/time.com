using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BASE.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;

namespace BASE.Infrastructure.Implements
{
    public static class PagedListExtensions
    {
        /// <summary>
        /// LIFESTYLE: IAppSettings is registered as singleton, so there's no need to dispose
        /// </summary>
        public static readonly int DefaultPageSize = 10;

        public static IPagedList<TItem> ToPagedList<TItem>(this IQueryable<TItem> queryable, int? pageIndex)
        {
            return queryable.ToPagedList(pageIndex: 1, pageSize: DefaultPageSize);
        }
        public static IPagedList<TItem> ToPagedList<TItem>(this IEnumerable<TItem> queryable, int pageIndex, int pageSize, int totalCount)
        {
            return queryable.ToPagedListIEnumerable(totalCount, pageIndex: 1, pageSize: DefaultPageSize);
        }
        public static IPagedList<TItem> ToPagedList<TItem>(
            this IQueryable<TItem> queryable,
            int pageIndex,
            int pageSize,
            bool nopaging = false)
        {
            return new AnonymousPagedList<TItem>(queryable, pageIndex, pageSize, nopaging);
        }
        public static IPagedList<TItem> ToPagedListIEnumerable<TItem>(
            this IEnumerable<TItem> queryable, int totalCount,
            int pageIndex,
            int pageSize
            )
        {
            return new AnonymousPagedList<TItem>(queryable, totalCount, pageIndex, pageSize);
        }

        private class AnonymousPagedList<TItem> : BasePagedList<TItem>
        {
            public AnonymousPagedList(
                IQueryable<TItem> queryable,
                int pageIndex,
                int pageSize,
                bool nopaging = false)
                : base(queryable, pageIndex, pageSize, nopaging)
            {
            }
            public AnonymousPagedList(IEnumerable<TItem> queryable, int totalPage, int pageIndex, int pageSize)
              : base(queryable, pageIndex, pageSize, totalPage)
            {
            }
        }
        private class AnonymousAsyncPagedList<TItem> : BasePagedList<TItem>
        {
            private const string _PAGE_NUMBER_BELOW_1 = "Page number cannot be below 1.";
            private const string _PAGE_SIZE_LESS_THAN_1 = "Page size cannot be less than 1.";

            private AnonymousAsyncPagedList()
            {
            }

            public static async Task<IPagedList<TItem>> CreateAsync(
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

                var result = new AnonymousAsyncPagedList<TItem>();

                await result.CreateCoreAsync(queryable, pageIndex, pageSize, nopaging);

                return result;
            }

            private async Task CreateCoreAsync(IQueryable<TItem> queryable, int pageIndex, int pageSize,
                bool nopaging = false)
            {
                if (nopaging)
                {
                    Items = await queryable.ToArrayAsync();
                    return;
                }

                TotalItemCount = await queryable.CountAsync();
                PageSize = pageSize;
                PageCount =
                    TotalItemCount > 0
                        ? (int)Math.Ceiling(TotalItemCount / (double)PageSize)
                        : 0;
                PageIndex = pageIndex > PageCount ? PageCount : pageIndex;
                HasPreviousPage = PageIndex > 1;
                HasNextPage = PageIndex < PageCount;


                Items = await queryable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArrayAsync();
            }
        }
    }
}
