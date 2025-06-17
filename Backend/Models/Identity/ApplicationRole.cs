using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Identity
{
    public class ApplicationRole : IdentityRole
    {
        [Required]
        [StringLength(256)]
        public override string? Name { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public override string? NormalizedName { get; set; } = string.Empty;
    }
}
