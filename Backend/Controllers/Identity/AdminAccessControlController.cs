using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Controllers.Identity
{
    /// <summary>
    /// API controller for managing role-based access control to modules.
    /// </summary>
    /// <remarks>
    /// Allows admin users to retrieve and update access permissions between roles and application modules.
    /// </remarks>
    [ApiController]
    [Route("api/admin/access-control")]
    public class AdminAccessControlController : ControllerBase
    {
        private readonly IAdminAccessControlService _accessControlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminAccessControlController"/> class.
        /// </summary>
        /// <param name="accessControlService">Service that handles access control logic.</param>
        public AdminAccessControlController(IAdminAccessControlService accessControlService)
        {
            _accessControlService = accessControlService;
        }

        /// <summary>
        /// Retrieves all current role-to-module access control mappings.
        /// </summary>
        /// <returns>A list of access control entries.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAccessControls()
        {
            var result = await _accessControlService.GetAccessControlsAsync();
            return Ok(result);
        }

        /// <summary>
        /// Updates the access permission for a specific role-module combination.
        /// </summary>
        /// <param name="dto">The access control data transfer object containing updated values.</param>
        /// <returns>
        /// HTTP 200 OK if the update was successful; otherwise, HTTP 400 Bad Request.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> UpdateAccessControl([FromBody] AccessControlDto dto)
        {
            var success = await _accessControlService.UpdateAccessControlAsync(dto);
            return success ? Ok(new { success = true }) : BadRequest(new { success = false });
        }
    }
}
