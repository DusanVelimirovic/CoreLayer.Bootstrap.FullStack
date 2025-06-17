namespace WebApp.DTOs.Identity
{
    /// <summary>
    /// Represents the response DTO containing module access information for a user.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used to communicate the list of module IDs the user is permitted to access.
    /// </remarks>
    public class ModuleAccessResponseDto
    {
        /// <summary>
        /// Gets or sets the list of module identifiers the user has access to.
        /// </summary>
        /// <value>A list of module IDs representing accessible modules.</value>
        public List<int> ModuleIds { get; set; } = new();
    }
}
