namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents a generic response returned from an API endpoint.
    /// </summary>
    /// <remarks>
    /// This DTO can be used to encapsulate success status and optional messages for API calls.
    /// </remarks>
    public class GenericApiResponseDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether the API request was successful.
        /// </summary>
        /// <value><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets an optional message associated with the response.
        /// </summary>
        /// <value>A descriptive message, often used for errors or confirmation feedback.</value>
        public string? Message { get; set; }
    }
}
