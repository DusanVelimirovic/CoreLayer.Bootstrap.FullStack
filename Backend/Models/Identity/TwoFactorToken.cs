namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents a Two-Factor Authentication (2FA) token used for verifying a user's identity.
    /// </summary>
    /// <remarks>
    /// Tokens are time-limited and linked to a specific user.
    /// </remarks>
    public class TwoFactorToken
    {
        /// <summary>
        /// Gets or sets the unique identifier for the token record.
        /// </summary>
        /// <value>Primary key of the 2FA token entry.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user to whom this token was issued.
        /// </summary>
        /// <value>Foreign key referencing the user.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the generated 2FA token.
        /// </summary>
        /// <value>A string representing the authentication code.</value>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration time of the token.
        /// </summary>
        /// <value>The UTC timestamp after which the token becomes invalid.</value>
        public DateTime Expiry { get; set; }

        /// <summary>
        /// Gets or sets the user associated with the token.
        /// </summary>
        /// <value>Navigation property to the <see cref="ApplicationUser"/>.</value>
        public ApplicationUser? User { get; set; }
    }
}
