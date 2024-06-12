using AutoMapper;
using BASE.Infrastructure.Implements;
using BASE.Infrastructure.Interface;

namespace BASE.Services
{
    public static class MapPageList
        {
            public static IPagedList<TItemDto> Map<TItemDto, TItem>(this IPagedList<TItem> pageList, IMapper _mapper)
            {
                IPagedList<TItemDto> result = new PagedList<TItemDto>
                {
                    HasNextPage = pageList.HasNextPage,
                    HasPreviousPage = pageList.HasPreviousPage,
                    Items = _mapper.Map<TItemDto[]>(pageList.Items),
                    PageCount = pageList.PageCount,
                    PageIndex = pageList.PageIndex,
                    PageSize = pageList.PageSize,
                    TotalItemCount = pageList.TotalItemCount
                };

                return result;
            }
        }

}
