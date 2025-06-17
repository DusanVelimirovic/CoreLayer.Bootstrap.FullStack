namespace Backend.Models.Identity
{
    /// <summary>
    /// Represents the access control rule associating a role with a module.
    /// </summary>
    /// <remarks>
    /// This defines whether a specific role has access to a given system module.
    /// </remarks>
    public class ModuleAccessControl
    {
        /// <summary>
        /// Gets or sets the unique identifier for the access control entry.
        /// </summary>
        /// <value>Primary key for the access control record.</value>
        public int ModuleAccessControlId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated role.
        /// </summary>
        /// <value>Foreign key to the <see cref="ApplicationRole"/>.</value>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role associated with this access control entry.
        /// </summary>
        /// <value>Navigation property to the role.</value>
        public ApplicationRole? Role { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated module.
        /// </summary>
        /// <value>Foreign key to the <see cref="ModuleI"/>.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the module associated with this access control entry.
        /// </summary>
        /// <value>Navigation property to the module.</value>
        public ModuleI? Module { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the role has access to the module.
        /// </summary>
        /// <value><c>true</c> if the role can access the module; otherwise, <c>false</c>.</value>
        public bool CanAccess { get; set; } = false;
    }
}
