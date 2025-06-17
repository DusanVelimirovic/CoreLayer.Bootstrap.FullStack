using Backend.Data;
using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Backend.Services.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Backend.Services.Identity
{
    /// <summary>
    /// Service for managing application modules and their metadata.
    /// </summary>
    /// <remarks>
    /// Provides operations to retrieve, create, and update modules.
    /// </remarks>
    public class ModuleService : IModuleService
    {
        private readonly CoreLayerDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleService"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public ModuleService(CoreLayerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all modules in the system, sorted by name.
        /// </summary>
        /// <returns>A list of <see cref="ModuleDto"/> objects representing available modules.</returns>
        public async Task<List<ModuleDto>> GetAllModulesAsync()
        {
            return await _context.Modules
                .OrderBy(m => m.ModuleName)
                .Select(m => new ModuleDto
                {
                    ModuleId = m.ModuleId,
                    ModuleName = m.ModuleName
                })
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new module if one with the same name does not already exist.
        /// </summary>
        /// <param name="dto">DTO containing the module name to be created.</param>
        /// <returns>
        /// The created <see cref="ModuleDto"/> if successful; <c>null</c> if a module with the same name already exists.
        /// </returns>
        public async Task<ModuleDto?> CreateModuleAsync(CreateModuleDto dto)
        {
            var exists = await _context.Modules.AnyAsync(m => m.ModuleName == dto.ModuleName);
            if (exists) return null;

            var module = new ModuleI { ModuleName = dto.ModuleName };
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();

            return new ModuleDto { ModuleId = module.ModuleId, ModuleName = module.ModuleName };
        }

        /// <summary>
        /// Updates the name of an existing module.
        /// </summary>
        /// <param name="dto">DTO containing the module ID and the new name.</param>
        /// <returns>
        /// The updated <see cref="ModuleDto"/> if the module was found and updated; otherwise, <c>null</c>.
        /// </returns>
        public async Task<ModuleDto?> UpdateModuleAsync(UpdateModuleDto dto)
        {
            var module = await _context.Modules.FindAsync(dto.ModuleId);
            if (module == null) return null;

            module.ModuleName = dto.ModuleName;
            await _context.SaveChangesAsync();

            return new ModuleDto { ModuleId = module.ModuleId, ModuleName = module.ModuleName };
        }
    }
}
