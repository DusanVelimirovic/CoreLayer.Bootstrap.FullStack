namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the authenticated user's session data.
    /// </summary>
    /// <remarks>
    /// This DTO is used to store minimal user context information such as identity, roles, and accessible modules,
    /// and is typically saved in browser storage for managing the authentication state in the frontend.
    /// </remarks>
    public class AuthUserSessionDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the authenticated user.
        /// </summary>
        /// <value>The user's unique ID.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the username of the authenticated user.
        /// </summary>
        /// <value>The display name or login name of the user.</value>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of role IDs assigned to the user.
        /// </summary>
        /// <value>A list of role identifiers.</value>
        public List<string> RoleIds { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of module IDs the user has access to.
        /// </summary>
        /// <value>A list of accessible module identifiers.</value>
        public List<int> AccessibleModuleIds { get; set; } = new();
    }
}
