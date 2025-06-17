namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to create a new role.
    /// </summary>
    /// <remarks>
    /// This DTO is used when submitting a request to create a new role in the system.
    /// </remarks>
    public class CreateRoleDto
    {
        /// <summary>
        /// Gets or sets the name of the role to be created.
        /// </summary>
        /// <value>The name of the new role.</value>
        public string Name { get; set; } = string.Empty;
    }
}
