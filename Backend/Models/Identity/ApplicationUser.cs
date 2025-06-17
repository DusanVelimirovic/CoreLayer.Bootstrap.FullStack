using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(256)]
        public override string? UserName { get; set; } = string.Empty;

        [StringLength(256)]
        public override string? Email { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public override bool TwoFactorEnabled { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserRoleAssignment> RoleAssignments { get; set; } = new List<UserRoleAssignment>();
    }
}
