using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Identity
{
    public class TrustedDevice
    {
        public int TrustedDeviceId { get; set; }

        [Required]
        [StringLength(256)]
        public string DeviceIdentifier { get; set; } = string.Empty;

        [Required]
        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public DateTime TrustedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiresOn { get; set; }
    }
}
