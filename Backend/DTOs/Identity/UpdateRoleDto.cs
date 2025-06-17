namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to update an existing role.
    /// </summary>
    /// <remarks>
    /// This DTO is used when modifying the name of a role in the system.
    /// </remarks>
    public class UpdateRoleDto
    {
        /// <summary>
        /// Gets or sets the new name for the role.
        /// </summary>
        /// <value>The updated role name.</value>
        public string Name { get; set; } = string.Empty;
    }
}
