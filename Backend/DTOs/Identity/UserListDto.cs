namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a simplified view of user information for display in user lists or administrative panels.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used in list views to display essential user details such as username, email, role, and status.
    /// </remarks>
    public class UserListDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <value>The user's ID.</value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        /// <value>The user's email.</value>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        /// <value>The user's username.</value>
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
        /// Gets or sets the list of role names assigned to the user.
        /// </summary>
        /// <value>A list of role names.</value>
        public List<string> Roles { get; set; } = new();
    }
}
