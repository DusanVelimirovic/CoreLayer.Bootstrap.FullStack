namespace Backend.DTOs.Identity
{
    public class UpdateUserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool? IsActive { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public string? RoleId { get; set; }
    }
}
