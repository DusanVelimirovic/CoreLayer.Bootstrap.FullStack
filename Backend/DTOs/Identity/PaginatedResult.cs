namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a generic paginated result set.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the paginated result.</typeparam>
    /// <remarks>
    /// Used for returning data with pagination metadata such as total count, page number, and page size.
    /// </remarks>
    public class PaginatedResult<T>
    {
        /// <summary>
        /// Gets or sets the list of items for the current page.
        /// </summary>
        /// <value>A list of items of type <typeparamref name="T"/>.</value>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        /// <value>An integer representing the total item count.</value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number (1-based index).
        /// </summary>
        /// <value>An integer representing the current page number.</value>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        /// <value>An integer representing the page size.</value>
        public int PageSize { get; set; }
    }
}
