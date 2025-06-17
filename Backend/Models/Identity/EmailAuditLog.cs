using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents an audit log entry for emails sent from the system.
    /// </summary>
    /// <remarks>
    /// Used for tracking email delivery attempts, including success/failure and metadata.
    /// </remarks>
    public class EmailAuditLog
    {
        /// <summary>
        /// Gets or sets the unique identifier for the email audit log entry.
        /// </summary>
        /// <value>Auto-incremented database ID.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the recipient's email address.
        /// </summary>
        /// <value>Email address to which the message was sent.</value>
        [Required]
        public string ToEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of email template used.
        /// </summary>
        /// <value>Template category like "2FA", "PasswordReset", etc.</value>
        [Required]
        public string TemplateType { get; set; } = string.Empty; // e.g. "2FA", "PasswordReset"

        /// <summary>
        /// Gets or sets the timestamp when the email was sent.
        /// </summary>
        /// <value>UTC datetime of the sending attempt.</value>
        [Required]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the email was successfully sent.
        /// </summary>
        /// <value>True if the email was sent without error; otherwise, false.</value>
        [Required]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the error message if the email failed to send.
        /// </summary>
        /// <value>Error message from the mail system, or null if successful.</value>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user associated with the email (optional).
        /// </summary>
        /// <value>Foreign key to the user; may be null for system emails.</value>
        public string? UserId { get; set; }

        /// <summary>
        /// Navigation property to the related user, if available.
        /// </summary>
        /// <value>ApplicationUser entity that the email was related to.</value>
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
