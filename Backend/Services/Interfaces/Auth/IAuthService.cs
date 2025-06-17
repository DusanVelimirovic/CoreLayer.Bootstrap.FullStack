using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);
        Task SaveLoginAttemptAsync(
            string userId,
            string? ipAddress,
            bool success,
            string? userAgent,
            string? failureReason,
            bool? is2FASuccess,
            string? deviceName,
            bool? isTrustedDevice);
        Task<bool> VerifyTwoFactorAsync(string userId, string token);
        Task<bool> MarkDeviceAsTrustedAsync(TrustedDeviceRequestDto dto);
        Task<List<LoginAuditLogResultDto>> GetLoginAuditLogsAsync(LoginAuditLogQueryDto queryDto);
        Task<bool> GeneratePasswordResetTokenAsync(string email);
        Task<bool> ValidatePasswordResetTokenAsync(string userId, string token);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);




    }
}
