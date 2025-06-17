using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Controllers.Identity
{
    [ApiController]
    [Route("api/admin/roles")]
    public class AdminRoleController : ControllerBase
    {
        /// <summary>
        /// API controller for managing user roles in the system.
        /// </summary>
        /// <remarks>
        /// Provides endpoints for retrieving, creating, and updating application roles for administrative purposes.
        /// </remarks>
        private readonly IAdminRoleService _roleService;


        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRoleController"/> class.
        /// </summary>
        /// <param name="roleService">Service for role management operations.</param>
        public AdminRoleController(IAdminRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Retrieves all roles available in the system.
        /// </summary>
        /// <returns>A list of role objects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="dto">The data transfer object containing role name.</param>
        /// <returns>Status 200 OK if creation is successful, otherwise 400 Bad Request.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var success = await _roleService.CreateRoleAsync(dto);
            return success ? Ok(new { success = true }) : BadRequest(new { success = false });
        }

        /// <summary>
        /// Updates an existing role by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the role to update.</param>
        /// <param name="dto">The data transfer object containing the updated role name.</param>
        /// <returns>Status 200 OK if update is successful, otherwise 404 Not Found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRoleDto dto)
        {
            var success = await _roleService.UpdateRoleAsync(id, dto);
            return success ? Ok(new { success = true }) : NotFound(new { success = false });
        }
    }
}
