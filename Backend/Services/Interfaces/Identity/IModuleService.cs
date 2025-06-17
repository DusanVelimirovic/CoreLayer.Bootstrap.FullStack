using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Identity
{
    /// <summary>
    /// Defines operations for managing application modules used for access control and feature segregation.
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// Retrieves all modules available in the system, typically for use in administrative access control.
        /// </summary>
        /// <returns>
        /// A list of <see cref="ModuleDto"/> representing all modules.
        /// </returns>
        Task<List<ModuleDto>> GetAllModulesAsync();

        /// <summary>
        /// Creates a new module if the module name is unique.
        /// </summary>
        /// <param name="dto">The data required to create the module.</param>
        /// <returns>
        /// The created <see cref="ModuleDto"/> if successful; otherwise, <c>null</c> if a module with the same name already exists.
        /// </returns>
        Task<ModuleDto?> CreateModuleAsync(CreateModuleDto dto);

        /// <summary>
        /// Updates the name of an existing module.
        /// </summary>
        /// <param name="dto">The updated module information.</param>
        /// <returns>
        /// The updated <see cref="ModuleDto"/> if the module exists; otherwise, <c>null</c>.
        /// </returns>
        Task<ModuleDto?> UpdateModuleAsync(UpdateModuleDto dto);
    }
}
