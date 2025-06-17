namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to assign a role to a user.
    /// </summary>
    /// <remarks>
    /// This DTO is used when assigning an existing role to a specific user.
    /// </remarks>
    public class AssignUserRoleDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <value>The ID of the user who is receiving the role.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        /// <value>The ID of the role to assign to the user.</value>
        public string RoleId { get; set; } = string.Empty;
    }
}
