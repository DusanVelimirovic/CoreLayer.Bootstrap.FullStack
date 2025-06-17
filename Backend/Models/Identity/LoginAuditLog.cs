namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents a log entry for a user login attempt.
    /// </summary>
    /// <remarks>
    /// Used to track login activity for auditing, security analysis, and troubleshooting.
    /// </remarks>
    public class LoginAuditLog
    {
        /// <summary>
        /// Gets or sets the unique identifier for the login audit log entry.
        /// </summary>
        /// <value>Auto-incremented database ID.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user attempting to log in.
        /// </summary>
        /// <value>Foreign key to the ApplicationUser.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the IP address from which the login was attempted.
        /// </summary>
        /// <value>IPv4 or IPv6 address in string format.</value>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the user agent string of the browser/device used.
        /// </summary>
        /// <value>Browser and system details from the HTTP request header.</value>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the login attempt.
        /// </summary>
        /// <value>UTC datetime when the login attempt occurred.</value>
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets whether the login attempt was successful.
        /// </summary>
        /// <value>True if the login succeeded, otherwise false.</value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets whether the 2FA check was successful (if applicable).
        /// </summary>
        /// <value>True if 2FA succeeded, false if it failed, or null if not used.</value>
        public bool? Is2FASuccess { get; set; }

        /// <summary>
        /// Gets or sets the reason for login failure (if applicable).
        /// </summary>
        /// <value>Error or validation message explaining failure, or null if successful.</value>
        public string? FailureReason { get; set; }

        /// <summary>
        /// Gets or sets the name of the device used for login (if provided).
        /// </summary>
        /// <value>Human-readable device name or null.</value>
        public string? DeviceName { get; set; }

        /// <summary>
        /// Gets or sets whether the login came from a trusted device.
        /// </summary>
        /// <value>True if the device is trusted, false if not, or null if unknown.</value>
        public bool? IsTrustedDevice { get; set; }

        /// <summary>
        /// Gets or sets the navigation property for the user associated with this login log.
        /// </summary>
        /// <value>ApplicationUser entity or null if the user is deleted or unknown.</value>
        public ApplicationUser? User { get; set; }
    }
}
