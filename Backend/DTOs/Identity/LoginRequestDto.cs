namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to initiate a user login request.
    /// </summary>
    /// <remarks>
    /// This DTO includes credentials and optional device identification for login auditing and security checks.
    /// </remarks>
    public class LoginRequestDto
    {
        /// <summary>
        /// Gets or sets the user's username or email address used for login.
        /// </summary>
        /// <value>A string containing either the username or email.</value>
        public string UserNameOrEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's login password.
        /// </summary>
        /// <value>A string representing the user's password.</value>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique device identifier (e.g., from localStorage).
        /// </summary>
        /// <value>A string that uniquely identifies the client device.</value>
        public string? DeviceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the human-readable name of the device.
        /// </summary>
        /// <value>A string that provides the device name, useful for auditing or display purposes.</value>
        public string? DeviceName { get; set; }
    }
}
