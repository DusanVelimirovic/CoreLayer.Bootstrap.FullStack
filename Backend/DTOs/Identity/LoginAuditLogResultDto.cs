namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a single record of a user login attempt used for audit purposes.
    /// </summary>
    /// <remarks>
    /// This DTO includes login result, user details, device information, and metadata used for auditing login attempts.
    /// </remarks>
    public class LoginAuditLogResultDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the audit log entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the login attempt.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's email address, if available.
        /// </summary>
        public string? UserEmail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the login was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the reason for login failure, if applicable.
        /// </summary>
        public string? FailureReason { get; set; }

        /// <summary>
        /// Gets or sets the IP address from which the login attempt originated.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the user agent string provided by the client's browser.
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the name of the device used for the login attempt, if known.
        /// </summary>
        public string? DeviceName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether two-factor authentication was successful.
        /// </summary>
        public bool? Is2FASuccess { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the device is marked as trusted.
        /// </summary>
        public bool? IsTrustedDevice { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the login attempt occurred.
        /// </summary>
        public DateTime LoginTime { get; set; }
    }
}
