using System.Text.Json.Serialization;

namespace WebApp.DTOs.Common
{
    /// <summary>
    /// Represents a flat paginated response structure containing an identifier and a collection of values.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used when deserializing paginated results where JSON structure uses "id" and "values" keys.
    /// </remarks>
    public class FlatPagedResponse<T>
    {
        /// <summary>
        /// Gets or sets the identifier for the paginated response.
        /// </summary>
        /// <value>A string that typically represents the response ID or context.</value>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of result values in the current page.
        /// </summary>
        /// <value>A list of items of type <typeparamref name="T"/> representing the flat collection of results.</value>
        [JsonPropertyName("values")]
        public List<T> Values { get; set; } = new(); // Match "values", not "$values"
    }

}
