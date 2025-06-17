namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to update an existing user's information.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used for administrative updates to user accounts, such as changing email, username, activation status, or role assignment.
    /// </remarks>
    public class UpdateUserDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to be updated.
        /// </summary>
        /// <value>The user's unique ID.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's updated email address.
        /// </summary>
        /// <value>The new email address, or null to leave unchanged.</value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the user's updated username.
        /// </summary>
        /// <value>The new username, or null to leave unchanged.</value>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the activation status of the user.
        /// </summary>
        /// <value>True if active, false if deactivated, or null to leave unchanged.</value>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets whether two-factor authentication is enabled for the user.
        /// </summary>
        /// <value>True to enable 2FA, false to disable, or null to leave unchanged.</value>
        public bool? TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the role ID to assign to the user.
        /// </summary>
        /// <value>The new role ID, or null to leave unchanged.</value>
        public string? RoleId { get; set; }
    }
}
