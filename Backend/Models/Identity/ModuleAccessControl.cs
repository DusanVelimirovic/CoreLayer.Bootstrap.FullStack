namespace Backend.Models.Identity
{
    public class ModuleAccessControl
    {
        public int ModuleAccessControlId { get; set; }

        public string RoleId { get; set; } = string.Empty;
        public ApplicationRole? Role { get; set; }

        public int ModuleId { get; set; }
        public ModuleI? Module { get; set; }

        public bool CanAccess { get; set; } = false;
    }
}
