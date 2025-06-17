using System.Text.Json.Serialization;

namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the response returned after a login attempt.
    /// </summary>
    /// <remarks>
    /// Contains the authentication result, user identity, roles, and accessible modules.
    /// </remarks>
    public class LoginResponseDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether the login attempt was successful.
        /// </summary>
        /// <value><c>true</c> if login was successful; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the optional message returned with the login response.
        /// </summary>
        /// <value>A message explaining the result of the login attempt.</value>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether two-factor authentication is required.
        /// </summary>
        /// <value><c>true</c> if 2FA is required; otherwise, <c>false</c>.</value>
        public bool RequiresTwoFactor { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the login.
        /// </summary>
        /// <value>The unique identifier of the authenticated user.</value>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the list of role IDs assigned to the user.
        /// </summary>
        /// <value>A list of role identifiers.</value>
        public List<string> RoleIds { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of module IDs that the user has access to.
        /// </summary>
        /// <value>A list of accessible module identifiers.</value>
        public List<int> ModuleIdsWithAccess { get; set; } = new();

        /// <summary>
        /// Helper class for deserializing nested role ID arrays.
        /// </summary>
        public class RoleWrapper
        {
            /// <summary>
            /// Gets or sets the deserialized list of role IDs.
            /// </summary>
            [JsonPropertyName("$values")]
            public List<string> Values { get; set; } = new();
        }

        /// <summary>
        /// Helper class for deserializing nested module ID arrays.
        /// </summary>
        public class ModuleWrapper
        {
            /// <summary>
            /// Gets or sets the deserialized list of module IDs.
            /// </summary>
            [JsonPropertyName("$values")]
            public List<int> Values { get; set; } = new();
        }
    }
}
