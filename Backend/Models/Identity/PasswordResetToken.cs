using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents a token used for password reset functionality.
    /// </summary>
    /// <remarks>
    /// Each token is linked to a user and has a limited validity window.
    /// </remarks>
    public class PasswordResetToken
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reset token entry.
        /// </summary>
        /// <value>Primary key for the token record.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user associated with the reset token.
        /// </summary>
        /// <value>Foreign key to the <see cref="ApplicationUser"/>.</value>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user associated with this token.
        /// </summary>
        /// <value>Navigation property to the user.</value>
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reset token string.
        /// </summary>
        /// <value>Token used to authenticate a password reset request.</value>
        [Required]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration time of the token.
        /// </summary>
        /// <value>UTC datetime after which the token is no longer valid.</value>
        [Required]
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the token has already been used.
        /// </summary>
        /// <value><c>true</c> if the token has been used; otherwise, <c>false</c>.</value>
        public bool IsUsed { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the token has already been used.
        /// </summary>
        /// <value><c>true</c> if the token has been used; otherwise, <c>false</c>.</value>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
