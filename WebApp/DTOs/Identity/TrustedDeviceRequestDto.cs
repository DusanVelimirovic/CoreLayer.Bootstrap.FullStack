namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents a request to register or validate a trusted device for a user.
    /// </summary>
    /// <remarks>
    /// This DTO is commonly used when implementing device-based authentication mechanisms or two-factor authentication bypass.
    /// </remarks>
    public class TrustedDeviceRequestDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <value>A string that uniquely identifies the user (typically a GUID).</value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier for the device.
        /// </summary>
        /// <value>A string representing the fingerprint or unique token for the user's device.</value>
        public string DeviceIdentifier { get; set; } = string.Empty;
    }
}
