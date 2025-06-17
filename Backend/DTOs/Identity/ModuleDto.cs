namespace Backend.DTOs.Identity
{
    /// <summary>
    /// Represents a module with its unique identifier and name.
    /// </summary>
    /// <remarks>
    /// Used for displaying or managing modules within the system.
    /// </remarks>
    public class ModuleDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the module.
        /// </summary>
        /// <value>An integer representing the module ID.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>A string representing the module's display name.</value>
        public string ModuleName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the data required to create a new module.
    /// </summary>
    /// <remarks>
    /// Used when submitting a request to create a new module entity.
    /// </remarks>
    public class CreateModuleDto
    {
        /// <summary>
        /// Gets or sets the name of the new module.
        /// </summary>
        /// <value>A string representing the module name.</value>
        public string ModuleName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the data required to update an existing module.
    /// </summary>
    /// <remarks>
    /// Used when modifying module details such as name.
    /// </remarks>
    public class UpdateModuleDto
    {
        /// <summary>
        /// Gets or sets the identifier of the module to update.
        /// </summary>
        /// <value>An integer representing the existing module ID.</value>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the new name of the module.
        /// </summary>
        /// <value>A string representing the updated module name.</value>
        public string ModuleName { get; set; } = string.Empty;
    }
}
