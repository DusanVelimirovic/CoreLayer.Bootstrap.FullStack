namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents a role within the application used for authorization purposes.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used to transfer role data between the client and server, including role management interfaces.
    /// </remarks>
    public class RoleDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        /// <value>A string representing the role's ID (usually a GUID).</value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name of the role.
        /// </summary>
        /// <value>The name of the role (e.g., "Admin", "Manager").</value>
        public string Name { get; set; } = string.Empty;
    }
}
