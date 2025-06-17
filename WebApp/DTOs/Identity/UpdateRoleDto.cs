namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to update an existing role.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used in administrative scenarios for renaming roles.
    /// </remarks>
    public class UpdateRoleDto
    {
        /// <summary>
        /// Gets or sets the new name of the role.
        /// </summary>
        /// <value>The updated name to assign to the role.</value>
        public string Name { get; set; } = string.Empty;
    }
}
