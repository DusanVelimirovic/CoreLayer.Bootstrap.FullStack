using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.Identity;
using Backend.Services.Interfaces.Identity;

namespace Backend.Controllers.Identity
{
    /// <summary>
    /// Provides API endpoints for managing application modules.
    /// </summary>
    /// <remarks>
    /// Supports operations for retrieving, creating, and updating modules.
    /// </remarks>
    [ApiController]
    [Route("api/admin/modules")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        // <summary>
        /// Initializes a new instance of the <see cref="ModuleController"/> class.
        /// </summary>
        /// <param name="moduleService">The module service used for handling module operations.</param>
        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        /// <summary>
        /// Retrieves all modules available in the system.
        /// </summary>
        /// <returns>A list of all <see cref="ModuleDto"/> objects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var modules = await _moduleService.GetAllModulesAsync();
            return Ok(modules);
        }

        /// <summary>
        /// Creates a new module in the system.
        /// </summary>
        /// <param name="dto">The module data for creation.</param>
        /// <returns>The newly created module or a conflict response if it already exists.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModuleDto dto)
        {
            var created = await _moduleService.CreateModuleAsync(dto);
            if (created == null)
                return Conflict(new { message = "Modul vec postoji." });

            return Ok(created);
        }

        /// <summary>
        /// Updates an existing module.
        /// </summary>
        /// <param name="dto">The updated module data.</param>
        /// <returns>The updated module or a not found response if the module doesn't exist.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateModuleDto dto)
        {
            var updated = await _moduleService.UpdateModuleAsync(dto);
            if (updated == null)
                return NotFound(new { message = "Modul nije pronadjen." });

            return Ok(updated);
        }
    }
}
