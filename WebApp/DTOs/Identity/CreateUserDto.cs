namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to create a new user.
    /// </summary>
    /// <remarks>
    /// This DTO is used when creating a user account along with the initial role assignment.
    /// </remarks>
    public class CreateUserDto
    {
        /// <summary>
        /// Gets or sets the username for the new user.
        /// </summary>
        /// <value>The unique username.</value>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address for the new user.
        /// </summary>
        /// <value>The user's email address.</value>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the new user.
        /// </summary>
        /// <value>The plaintext password (should be hashed before storage).</value>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the role to assign to the user.
        /// </summary>
        /// <value>The identifier of the role the user will belong to.</value>
        public string RoleId { get; set; } = string.Empty;
    }
}
