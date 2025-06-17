namespace Backend.DTOs.Identity
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public string? UserId { get; set; }

        public List<string> RoleIds { get; set; } = new();
        public List<string> RoleNames { get; set; } = new();
        public List<int> ModuleIdsWithAccess { get; set; } = new();
        public int? RemainingAttempts { get; set; }
    }
}
