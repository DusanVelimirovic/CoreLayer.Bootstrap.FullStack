using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Identity
{
    public class ModuleI
    {
        [Key]
        public int ModuleId { get; set; }

        [Required]
        [StringLength(100)]
        public string ModuleName { get; set; } = string.Empty;

        public ICollection<ModuleAccessControl> AccessControls { get; set; } = new List<ModuleAccessControl>();
    }
}
