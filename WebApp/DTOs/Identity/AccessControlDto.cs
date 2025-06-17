namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the access control rule defining whether a specific role can access a module.
    /// </summary>
    /// <remarks>
    /// This DTO is used to configure role-based access to different application modules.
    /// </remarks>
    public class AccessControlDto
    {
        /// <summary>
        /// Gets or sets the ID of the module associated with the access control entry.
        /// </summary>
        /// <value>The unique identifier of the module.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the role that the access control applies to.
        /// </summary>
        /// <value>The unique identifier of the role.</value>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the role has access to the module.
        /// </summary>
        /// <value><c>true</c> if access is granted; otherwise, <c>false</c>.</value>
        public bool CanAccess { get; set; }
    }
}
