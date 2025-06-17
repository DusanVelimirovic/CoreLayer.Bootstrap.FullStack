using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Controllers.Identity
{
    /// <summary>
    /// API controller for managing user accounts in the admin panel.
    /// </summary>
    /// <remarks>
    /// Provides endpoints for retrieving, creating, updating users and assigning roles.
    /// </remarks>
    [ApiController]
    [Route("api/admin/users")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserController"/> class.
        /// </summary>
        /// <param name="adminUserService">Service used for user administration operations.</param>
        public AdminUserController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        /// <summary>
        /// Retrieves a paginated list of users with optional search.
        /// </summary>
        /// <param name="page">Page number for pagination.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="search">Optional search filter by username or email.</param>
        /// <returns>A paginated list of users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var result = await _adminUserService.GetUsersAsync(page, pageSize, search);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing user's profile and settings.
        /// </summary>
        /// <param name="userId">ID of the user to update.</param>
        /// <param name="dto">DTO containing updated user data.</param>
        /// <returns>Status 200 OK if updated, otherwise 400 or 404 with message.</returns>
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto dto)
        {
            if (userId != dto.UserId)
                return BadRequest("Netacna sifra korisnika.");

            var success = await _adminUserService.UpdateUserAsync(dto);
            return success ? Ok(new { success = true }) : NotFound(new { success = false, message = "Korisnik nije pronadjen ili pokusaj azuriranja je neuspesan." });
        }

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="dto">DTO containing the user registration data.</param>
        /// <returns>Status 200 OK if successful, otherwise 400 Bad Request with error message.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var (success, error) = await _adminUserService.CreateUserAsync(dto);

            if (!success)
                return BadRequest(new { success = false, message = error });

            return Ok(new { success = true, message = "Korisnik uspesno kreiran." });
        }

        /// <summary>
        /// Assigns a role to an existing user.
        /// </summary>
        /// <param name="userId">ID of the user to assign the role to.</param>
        /// <param name="dto">DTO containing user ID and role ID.</param>
        /// <returns>Status 200 OK if successful, otherwise 400 Bad Request.</returns>
        // Asign role to user endpoint
        [HttpPost("{userId}/assign-role")]
        public async Task<IActionResult> AssignRole(string userId, [FromBody] AssignUserRoleDto dto)
        {
            if (userId != dto.UserId)
                return BadRequest("Sifra korisnika netacna.");

            var success = await _adminUserService.AssignRoleToUserAsync(dto.UserId, dto.RoleId);

            if (!success)
                return BadRequest("Neuspeo pokusaj dodeljivanja role.");

            return Ok(new { success = true, message = "Uspesno dodeljena rola." });
        }
    }
}
