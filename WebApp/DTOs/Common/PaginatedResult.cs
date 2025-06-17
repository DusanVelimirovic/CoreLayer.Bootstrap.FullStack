using System.Text.Json.Serialization;

namespace WebApp.DTOs.Common
{
    /// <summary>
    /// Represents a paginated result containing items and pagination metadata.
    /// </summary>
    /// <typeparam name="T">The type of items contained in the paginated result.</typeparam>
    /// <remarks>
    /// This model is commonly used for returning paginated data from API endpoints along with total count and paging information.
    /// </remarks>
    public class PaginatedResult<T>
    {
        /// <summary>
        /// Gets or sets the collection of items returned on the current page.
        /// </summary>
        /// <value>A list of <typeparamref name="T"/> items.</value>
        [JsonPropertyName("items")]
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Gets or sets the total number of records available (alias to <see cref="TotalCount"/>).
        /// </summary>
        /// <value>The total number of records.</value>
        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the total number of records (alias to <see cref="TotalRecords"/>).
        /// </summary>
        /// <value>Total number of items across all pages.</value>
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number (starts from 1).
        /// </summary>
        /// <value>The current page index.</value>
        [JsonPropertyName("page")]
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the current page number (JSON alias for <see cref="Page"/>).
        /// </summary>
        /// <value>The current page.</value>
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        /// <value>Page size (number of items per page).</value>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }
}