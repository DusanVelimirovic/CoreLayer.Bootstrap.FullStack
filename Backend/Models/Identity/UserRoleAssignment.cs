namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents an assignment of a role to a user within the system.
    /// </summary>
    /// <remarks>
    /// Each assignment links a user to a role with a timestamp indicating when the role was assigned.
    /// </remarks>
    public class UserRoleAssignment
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user-role assignment.
        /// </summary>
        /// <value>Primary key of the assignment entry.</value>
        public int UserRoleAssignmentId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user receiving the role.
        /// </summary>
        /// <value>Foreign key referencing the user.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user associated with this role assignment.
        /// </summary>
        /// <value>Navigation property to the <see cref="ApplicationUser"/>.</value>
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Gets or sets the ID of the assigned role.
        /// </summary>
        /// <value>Foreign key referencing the role.</value>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role assigned to the user.
        /// </summary>
        /// <value>Navigation property to the <see cref="ApplicationRole"/>.</value>
        public ApplicationRole? Role { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the role was assigned.
        /// </summary>
        /// <value>Timestamp of role assignment.</value>
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
