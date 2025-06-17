namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to create a new user account.
    /// </summary>
    /// <remarks>
    /// This DTO is used during user creation, typically by an administrator assigning a role and credentials.
    /// </remarks>
    public class CreateUserDto
    {
        /// <summary>
        /// Gets or sets the username for the new user.
        /// </summary>
        /// <value>The login username.</value>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address for the new user.
        /// </summary>
        /// <value>The user's email.</value>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the new user.
        /// </summary>
        /// <value>The plain-text password (should be hashed during processing).</value>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the role assigned to the new user.
        /// </summary>
        /// <value>The identifier of the role.</value>
        public string RoleId { get; set; } = string.Empty;
    }
}
