using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Identity
{
    public class EmailAuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ToEmail { get; set; } = string.Empty;

        [Required]
        public string TemplateType { get; set; } = string.Empty; // e.g. "2FA", "PasswordReset"

        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
