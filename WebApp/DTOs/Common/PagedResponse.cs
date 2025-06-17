using System.Text.Json.Serialization;

namespace WebApp.DTOs.Common
{
    /// <summary>
    /// Represents a standard paginated response with metadata and a collection of items.
    /// </summary>
    /// <typeparam name="T">The type of the data elements being paginated.</typeparam>
    /// <remarks>
    /// This model accommodates complex paginated API responses with both metadata and nested values structure.
    /// </remarks>
    public class PagedResponse<T>
    {
        /// <summary>
        /// Gets or sets the nested paged data object (for alternate deserialization).
        /// </summary>
        /// <value>A <see cref="PagedData{T}"/> containing the actual items.</value>
        public PagedData<T> Data { get; set; } = new();

        /// <summary>
        /// Gets or sets the unique identifier for the response (used in JSON metadata).
        /// </summary>
        /// <value>A string representing the response ID.</value>
        [JsonPropertyName("$id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the paged items (used when the JSON response uses "items" as the container).
        /// </summary>
        /// <value>A <see cref="PagedData{T}"/> containing item values.</value>
        [JsonPropertyName("items")]
        public PagedData<T> Items { get; set; } = new();

        /// <summary>
        /// Gets or sets the current page index.
        /// </summary>
        /// <value>The current page number, starting from 1.</value>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the current page number as defined in JSON (alias to <see cref="Page"/>).
        /// </summary>
        /// <value>The current page index.</value>
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        /// <value>An integer defining the page size.</value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total number of records across all pages.
        /// </summary>
        /// <value>The total item count.</value>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages available.
        /// </summary>
        /// <value>Total number of paginated pages.</value>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the flattened list of paginated values (used in alternate JSON format).
        /// </summary>
        /// <value>A list of <typeparamref name="T"/> representing the actual results.</value>
        [JsonPropertyName("$values")]
        public List<T> Values { get; set; } = new(); // Match the outer "$values"

    }

    /// <summary>
    /// Represents a container for paged values, typically nested inside paged responses.
    /// </summary>
    /// <typeparam name="T">The type of each item in the collection.</typeparam>
    public class PagedData<T>
    {
        /// <summary>
        /// Gets or sets the list of items contained in the paginated structure.
        /// </summary>
        /// <value>A list of <typeparamref name="T"/> items.</value>
        [JsonPropertyName("$values")]
        public List<T> Values { get; set; } = new();
    }
}
