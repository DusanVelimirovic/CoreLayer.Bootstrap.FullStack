namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a user role in the system.
    /// </summary>
    /// <remarks>
    /// This DTO is used to transfer role information such as its ID and display name.
    /// </remarks>
    public class RoleDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        /// <value>The role's unique string ID.</value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name of the role.
        /// </summary>
        /// <value>The human-readable name of the role (e.g., "Administrator").</value>
        public string Name { get; set; } = string.Empty;
    }
}
