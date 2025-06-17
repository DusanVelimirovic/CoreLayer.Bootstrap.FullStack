namespace WebApp.DTOs.Identity
{

    /// <summary>
    /// Represents a module with its unique identifier and name.
    /// </summary>
    /// <remarks>
    /// This DTO is used to transfer basic module information, such as in listings or access control views.
    /// </remarks>
    public class ModuleDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the module.
        /// </summary>
        /// <value>The unique numeric ID of the module.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the module.
        /// </summary>
        /// <value>The name used to identify the module in the UI.</value>
        public string ModuleName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a DTO used when creating a new module.
    /// </summary>
    /// <remarks>
    /// Used in API requests or form submissions to define a new module by name.
    /// </remarks>
    public class CreateModuleDto
    {
        /// <summary>
        /// Gets or sets the name of the module to be created.
        /// </summary>
        /// <value>The name of the new module.</value>
        public string ModuleName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a DTO used when updating an existing module.
    /// </summary>
    /// <remarks>
    /// Used to rename or update a module by providing its ID and new name.
    /// </remarks>
    public class UpdateModuleDto
    {
        /// <summary>
        /// Gets or sets the identifier of the module to be updated.
        /// </summary>
        /// <value>The unique numeric ID of the module.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the new name for the module.
        /// </summary>
        /// <value>The updated name for the module.</value>
        public string ModuleName { get; set; } = string.Empty;
    }
}
