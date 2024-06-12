namespace BASE.Infrastructure.Interface
{
    public interface IPagedList<TItem>
    {
        TItem[] Items { get; set; }

        /// <summary>
        /// Total number of subsets within the superset.
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// Total number of objects contained within the superset.
        /// </summary>
        int TotalItemCount { get; set; }

        /// <summary>
        /// One-based index of this subset within the superset.
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// Maximum size of any individual subset.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Returns true if this is NOT the first subset within the superset.
        /// </summary>
        bool HasPreviousPage { get; set; }

        /// <summary>
        /// Returns true if this is NOT the last subset within the superset.
        /// </summary>
        bool HasNextPage { get; set; }
        /// <summary>
        /// Set Id of folder which contain map files.
        /// </summary>
        long? CgisId { set; get; }

    }
}
