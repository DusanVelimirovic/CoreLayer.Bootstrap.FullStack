using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Identity;
using Backend.Services.Interfaces.Auth;

namespace Backend.Controllers.Identity
{

    /// <summary>
    /// API controller for administrative access to login audit logs.
    /// </summary>
    /// <remarks>
    /// Provides endpoints for querying user login history and audit trail based on filters.
    /// </remarks>
    [ApiController]
    [Route("api/admin/audit")]
    public class AdminAuditController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminAuditController"/> class.
        /// </summary>
        /// <param name="authService">Service for accessing authentication audit logs.</param>
        public AdminAuditController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Retrieves login audit logs based on provided query filters.
        /// </summary>
        /// <param name="query">The query DTO containing filter parameters like date range, user ID, etc.</param>
        /// <returns>A filtered list of login audit log entries.</returns>
        [HttpPost("login-logs")]
        public async Task<IActionResult> GetAuditLogs([FromBody] LoginAuditLogQueryDto query)
        {
            var logs = await _authService.GetLoginAuditLogsAsync(query);
            return Ok(logs);
        }
    }
}
