using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Services.Auth
{
    public class TwoFactorCleanupService
    {
        private readonly CoreLayerDbContext _context;
        private readonly ILogger<TwoFactorCleanupService> _logger;

        public TwoFactorCleanupService(CoreLayerDbContext context, ILogger<TwoFactorCleanupService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> CleanupExpiredTokensAsync()
        {
            var now = DateTime.UtcNow;
            var expiredTokens = await _context.TwoFactorTokens
                .Where(t => t.Expiry < now)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.TwoFactorTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
                _logger.LogInformation($" Removed {expiredTokens.Count} expired 2FA tokens.");
            }

            return expiredTokens.Count;
        }
    }
}
