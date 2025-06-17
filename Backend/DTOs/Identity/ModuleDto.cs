namespace Backend.DTOs.Identity
{
    public class ModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
    }

    public class CreateModuleDto
    {
        public string ModuleName { get; set; } = string.Empty;
    }

    public class UpdateModuleDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
    }
}
