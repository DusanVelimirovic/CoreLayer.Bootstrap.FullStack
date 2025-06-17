namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to update an existing user account.
    /// </summary>
    /// <remarks>
    /// This DTO is used for modifying user details such as email, username, activation status,
    /// 2FA preference, and assigned role.
    /// </remarks>
    public class UpdateUserDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to be updated.
        /// </summary>
        /// <value>The ID of the user.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <value>The updated email address.</value>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        /// <value>The updated username.</value>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        /// <value><c>true</c> if the user is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether two-factor authentication is enabled for the user.
        /// </summary>
        /// <value><c>true</c> if 2FA is enabled; otherwise, <c>false</c>.</value>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the role identifier assigned to the user.
        /// </summary>
        /// <value>The ID of the assigned role, or null if not specified.</value>
        public string? RoleId { get; set; }
    }
}
