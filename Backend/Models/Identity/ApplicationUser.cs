using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents an application user with extended properties from ASP.NET IdentityUser.
    /// </summary>
    /// <remarks>
    /// Adds IsActive flag, creation timestamp, and user role assignments.
    /// </remarks>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        /// <value>Unique username used for login.</value>
        [Required]
        [StringLength(256)]
        public override string? UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <value>User's email address.</value>
        [StringLength(256)]
        public override string? Email { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user account is active.
        /// </summary>
        /// <value>True if the account is active; otherwise, false.</value>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indicates whether two-factor authentication is enabled.
        /// </summary>
        /// <value>True if 2FA is enabled; otherwise, false.</value>
        public override bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the user account.
        /// </summary>
        /// <value>UTC timestamp when the account was created.</value>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the collection of user-role assignments.
        /// </summary>
        /// <value>List of roles assigned to the user.</value>
        public ICollection<UserRoleAssignment> RoleAssignments { get; set; } = new List<UserRoleAssignment>();
    }
}
