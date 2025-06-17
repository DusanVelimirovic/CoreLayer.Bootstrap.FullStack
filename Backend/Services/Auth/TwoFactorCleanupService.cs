using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Services.Auth
{
    /// <summary>
    /// Service responsible for cleaning up expired two-factor authentication tokens from the database.
    /// </summary>
    /// <remarks>
    /// Typically used as a background task or scheduled job to maintain token hygiene.
    /// </remarks>
    public class TwoFactorCleanupService
    {
        private readonly CoreLayerDbContext _context;
        private readonly ILogger<TwoFactorCleanupService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoFactorCleanupService"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        /// <param name="logger">Logger used for audit and debugging purposes.</param>
        public TwoFactorCleanupService(CoreLayerDbContext context, ILogger<TwoFactorCleanupService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Removes all expired two-factor authentication tokens from the database.
        /// </summary>
        /// <returns>
        /// The number of tokens that were deleted.
        /// </returns>
        /// <remarks>
        /// Tokens with expiry timestamps earlier than the current UTC time are considered expired.
        /// </remarks>
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
