namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a request to register a trusted device for a user.
    /// </summary>
    /// <remarks>
    /// This DTO is used during 2FA to mark a specific device as trusted, bypassing future authentication challenges.
    /// </remarks>
    public class TrustedDeviceRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <value>The user's unique ID.</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the device.
        /// </summary>
        /// <value>A string token that uniquely identifies the device.</value>
        public string DeviceIdentifier { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the friendly name of the device.
        /// </summary>
        /// <value>An optional name to describe the device (e.g., "John's Laptop").</value>
        public string? DeviceName { get; set; }
    }
}
