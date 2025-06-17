namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents query parameters used to filter login audit logs.
    /// </summary>
    /// <remarks>
    /// This DTO allows filtering by user identifier, success status, date range, and pagination.
    /// </remarks>
    public class LoginAuditLogQueryDto
    {
        /// <summary>
        /// Gets or sets the user ID or email to filter logs.
        /// </summary>
        /// <value>A user identifier or email address.</value>
        public string? UserIdOrEmail { get; set; }

        /// <summary>
        /// Gets or sets whether to filter by login success.
        /// </summary>
        /// <value>True for successful logins, false for failures, or null for all.</value>
        public bool? Success { get; set; }

        /// <summary>
        /// Gets or sets the start date of the login log search.
        /// </summary>
        /// <value>The beginning of the date range.</value>
        public DateTime? From { get; set; }

        /// <summary>
        /// Gets or sets the end date of the login log search.
        /// </summary>
        /// <value>The end of the date range.</value>
        public DateTime? To { get; set; }

        /// <summary>
        /// Gets or sets the page number for pagination.
        /// </summary>
        /// <value>The current page (default is 1).</value>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of results per page.
        /// </summary>
        /// <value>The size of the result page (default is 20).</value>
        public int PageSize { get; set; } = 20;
    }
}
