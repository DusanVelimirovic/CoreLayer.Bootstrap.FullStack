using System.Text.Json.Serialization;

namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents a simplified view of a user for administrative listing purposes.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used when displaying users in management tables with
    /// key identity and status properties.
    /// </remarks>
    public class UserListDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <value>The user ID.</value>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        /// <value>The email address associated with the user.</value>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's display name or login username.
        /// </summary>
        /// <value>The user name.</value>
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the user account is active.
        /// </summary>
        /// <value><c>true</c> if the account is active; otherwise, <c>false</c>.</value>
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether two-factor authentication is enabled for the user.
        /// </summary>
        /// <value><c>true</c> if 2FA is enabled; otherwise, <c>false</c>.</value>
        [JsonPropertyName("twoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the list of role names assigned to the user.
        /// </summary>
        /// <value>A list of role names.</value>
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();

        /// <summary>
        /// Gets or sets the ID of the primary role assigned to the user.
        /// </summary>
        /// <value>The role ID.</value>
        public string RoleId { get; set; } = string.Empty;
    }

}
