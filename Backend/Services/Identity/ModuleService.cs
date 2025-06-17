using Backend.Data;
using Backend.DTOs.Identity;
using Backend.Models.Identity;
using Backend.Services.Interfaces.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Backend.Services.Identity
{
    public class ModuleService : IModuleService
    {
        private readonly CoreLayerDbContext _context;

        public ModuleService(CoreLayerDbContext context)
        {
            _context = context;
        }

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

        public async Task<ModuleDto?> CreateModuleAsync(CreateModuleDto dto)
        {
            var exists = await _context.Modules.AnyAsync(m => m.ModuleName == dto.ModuleName);
            if (exists) return null;

            var module = new ModuleI { ModuleName = dto.ModuleName };
            _context.Modules.Add(module);
            await _context.SaveChangesAsync();

            return new ModuleDto { ModuleId = module.ModuleId, ModuleName = module.ModuleName };
        }

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
