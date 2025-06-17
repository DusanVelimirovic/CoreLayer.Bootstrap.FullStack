namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents access control permissions for a specific module and role.
    /// </summary>
    /// <remarks>
    /// Used to define or update whether a given role has access to a specific module.
    /// </remarks>
    public class AccessControlDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the module.
        /// </summary>
        /// <value>The ID of the module for which access is being configured.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the role.
        /// </summary>
        /// <value>The ID of the role being granted or denied access.</value>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether access is granted.
        /// </summary>
        /// <value><c>true</c> if the role has access to the module; otherwise, <c>false</c>.</value>
        public bool CanAccess { get; set; }
    }
}
