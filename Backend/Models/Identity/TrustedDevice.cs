using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents a trusted device for a specific user to bypass Two-Factor Authentication (2FA).
    /// </summary>
    /// <remarks>
    /// Devices are uniquely identified and can have an expiration period.
    /// </remarks>
    public class TrustedDevice
    {
        /// <summary>
        /// Gets or sets the unique identifier for the trusted device entry.
        /// </summary>
        /// <value>Primary key of the trusted device record.</value>
        public int TrustedDeviceId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the device (e.g. hardware or browser fingerprint).
        /// </summary>
        /// <value>A string used to recognize the device in future logins.</value>
        [Required]
        [StringLength(256)]
        public string DeviceIdentifier { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the user who trusted the device.
        /// </summary>
        /// <value>Foreign key referencing the user.</value>
        [Required]
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who owns the trusted device.
        /// </summary>
        /// <value>Navigation property to the <see cref="ApplicationUser"/>.</value>
        public ApplicationUser? User { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp when the device was marked as trusted.
        /// </summary>
        /// <value>Date and time of trust registration.</value>
        public DateTime TrustedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the optional expiration date for the trusted device.
        /// </summary>
        /// <value>If set, the device will no longer be trusted after this date.</value>
        public DateTime? ExpiresOn { get; set; }
    }
}
