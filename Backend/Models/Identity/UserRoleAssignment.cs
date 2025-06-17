namespace Backend.Models.Identity
{
    public class UserRoleAssignment
    {
        public int UserRoleAssignmentId { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public string RoleId { get; set; } = string.Empty;
        public ApplicationRole? Role { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
