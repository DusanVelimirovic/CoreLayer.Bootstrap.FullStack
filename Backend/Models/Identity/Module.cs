using System.ComponentModel.DataAnnotations;

namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents a system module that can be assigned to user roles for access control.
    /// </summary>
    /// <remarks>
    /// Each module corresponds to a functional unit or feature that can be protected by role-based access.
    /// </remarks>
    public class ModuleI
    {
        /// <summary>
        /// Gets or sets the unique identifier for the module.
        /// </summary>
        /// <value>Primary key for the module.</value>
        [Key]
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>A unique, human-readable name representing the module.</value>
        [Required]
        [StringLength(100)]
        public string ModuleName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of access control rules associated with this module.
        /// </summary>
        /// <value>List of access control entries linking roles to this module.</value>
        public ICollection<ModuleAccessControl> AccessControls { get; set; } = new List<ModuleAccessControl>();
    }
}
