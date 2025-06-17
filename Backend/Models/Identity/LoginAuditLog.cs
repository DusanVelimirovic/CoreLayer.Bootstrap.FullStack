namespace Backend.Models.Identity
{
    public class LoginAuditLog
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public DateTime LoginTime { get; set; } = DateTime.UtcNow;

        public bool Success { get; set; }

        public bool? Is2FASuccess { get; set; }

        public string? FailureReason { get; set; }

        public string? DeviceName { get; set; }

        public bool? IsTrustedDevice { get; set; }

        // Navigation
        public ApplicationUser? User { get; set; }
    }
}
