using Backend.DTOs.Identity;

namespace Backend.Services.Interfaces.Identity
{
    public interface IModuleService
    {
        Task<List<ModuleDto>> GetAllModulesAsync();
        Task<ModuleDto?> CreateModuleAsync(CreateModuleDto dto);
        Task<ModuleDto?> UpdateModuleAsync(UpdateModuleDto dto);
    }
}
