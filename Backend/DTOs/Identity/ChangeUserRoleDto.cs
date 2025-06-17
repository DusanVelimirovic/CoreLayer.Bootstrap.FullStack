namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents the data required to change a user's role.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used in scenarios where a user's current role needs to be updated to a new one.
    /// </remarks>
    public class ChangeUserRoleDto
    {
        /// <summary>
        /// Gets or sets the ID of the new role to assign to the user.
        /// </summary>
        /// <value>The unique identifier of the new role.</value>
        public string NewRoleId { get; set; } = string.Empty;
    }
}
