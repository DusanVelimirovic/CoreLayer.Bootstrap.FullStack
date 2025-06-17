using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents a custom application role derived from ASP.NET IdentityRole.
    /// </summary>
    /// <remarks>
    /// Includes validation attributes for the role name and normalized name.
    /// </remarks>
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The role's display name.</value>
        [Required]
        [StringLength(256)]
        public override string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the normalized name of the role.
        /// </summary>
        /// <value>The normalized name used for lookups.</value>
        [Required]
        [StringLength(256)]
        public override string? NormalizedName { get; set; } = string.Empty;
    }
}
