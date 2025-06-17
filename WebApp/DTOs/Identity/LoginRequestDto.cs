using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the login request payload containing user credentials.
    /// </summary>
    /// <remarks>
    /// Used during the authentication process to validate user credentials and optionally identify the device.
    /// </remarks>
    public class LoginRequestDto
    {
        /// <summary>
        /// Gets or sets the username or email address used to authenticate the user.
        /// </summary>
        /// <value>The user's login identifier.</value>
        [Required(ErrorMessage = "Korisničko ime ili email su obavezni.")]
        public string UserNameOrEmail { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the password for user authentication.
        /// </summary>
        /// <value>The user's password.</value>
        [Required(ErrorMessage = "Lozinka je obavezna.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the optional device identifier for tracking the login origin.
        /// </summary>
        /// <value>A string representing the unique device identifier.</value>
        public string? DeviceIdentifier { get; set; }
    }
}
