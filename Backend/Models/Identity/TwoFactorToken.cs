namespace Backend.Models.Identity
{
    public class TwoFactorToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }

        public ApplicationUser? User { get; set; }
    }
}
