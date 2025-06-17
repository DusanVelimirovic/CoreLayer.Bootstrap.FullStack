namespace Backend.DTOs.Identity
{
    public class TwoFactorVerificationDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
