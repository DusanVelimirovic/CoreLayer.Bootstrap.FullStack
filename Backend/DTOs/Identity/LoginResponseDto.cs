namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the response returned after a user login attempt.
    /// </summary>
    /// <remarks>
    /// This DTO is used to convey the result of the login operation, including user access, roles, and security requirements.
    /// </remarks>
    public class LoginResponseDto
    {
        /// <summary>
        /// Indicates whether the login was successful.
        /// </summary>
        /// <value><c>true</c> if the login was successful; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        /// <summary>
        /// Contains a message describing the result of the login attempt.
        /// </summary>
        /// <value>A string containing a success, error, or informational message.</value>
        public string? Message { get; set; }

        /// <summary>
        /// Indicates whether two-factor authentication is required to complete the login.
        /// </summary>
        /// <value><c>true</c> if 2FA is required; otherwise, <c>false</c>.</value>
        public bool RequiresTwoFactor { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the logged-in user.
        /// </summary>
        /// <value>A string representing the user ID, if available.</value>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the list of role IDs assigned to the user.
        /// </summary>
        /// <value>A list of strings representing the user's roles by ID.</value>
        public List<string> RoleIds { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of role names assigned to the user.
        /// </summary>
        /// <value>A list of strings representing the user's roles by name.</value>
        public List<string> RoleNames { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of module IDs the user has access to.
        /// </summary>
        /// <value>A list of integers representing accessible modules for the user.</value>
        public List<int> ModuleIdsWithAccess { get; set; } = new();

        /// <summary>
        /// Gets or sets the number of remaining login attempts before the account is locked (if applicable).
        /// </summary>
        /// <value>An integer representing the remaining attempts, or <c>null</c> if not tracked.</value>
        public int? RemainingAttempts { get; set; }
    }
}
