namespace Backend.DTOs.Identity
{
    public class TrustedDeviceRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string DeviceIdentifier { get; set; } = string.Empty;
        public string? DeviceName { get; set; }
    }
}
