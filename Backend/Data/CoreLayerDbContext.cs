using Backend.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    /// <summary>
    /// Represents the Entity Framework Core database context for CoreLayer.
    /// Inherits from <see cref="IdentityDbContext{TUser, TRole, TKey}"/> to support ASP.NET Core Identity.
    /// </summary>
    /// <remarks>
    /// This context configures identity entities, audit logging, module access control, and various system-level constraints and indexes.
    /// </remarks>
    public class CoreLayerDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreLayerDbContext"/> class.
        /// </summary>
        /// <param name="options">The configuration options for the DbContext.</param>
        public CoreLayerDbContext(DbContextOptions<CoreLayerDbContext> options)
            : base(options) { }

        // DbSet declarations

        /// <summary>Gets or sets the users in the application.</summary>
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        /// <summary>Gets or sets the roles in the application.</summary>
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        /// <summary>Gets or sets the available system modules.</summary>
        public DbSet<ModuleI> Modules { get; set; }

        /// <summary>Gets or sets access control rules for modules and roles.</summary>
        public DbSet<ModuleAccessControl> ModuleAccessControls { get; set; }

        /// <summary>Gets or sets the assignments between users and roles.</summary>
        public DbSet<UserRoleAssignment> UserRoleAssignments { get; set; }

        /// <summary>Gets or sets the login audit logs.</summary>
        public DbSet<LoginAuditLog> LoginAuditLogs { get; set; }

        /// <summary>Gets or sets the two-factor authentication tokens.</summary>
        public DbSet<TwoFactorToken> TwoFactorTokens { get; set; }

        /// <summary>Gets or sets the trusted devices for users.</summary>
        public DbSet<TrustedDevice> TrustedDevices { get; set; }

        /// <summary>Gets or sets the password reset tokens.</summary>
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        /// <summary>Gets or sets the audit logs for emails sent to users.</summary>
        public DbSet<EmailAuditLog> EmailAuditLogs { get; set; }

        /// <summary>
        /// Configures the entity relationships and constraints using the Fluent API.
        /// </summary>
        /// <param name="modelBuilder">The builder used to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships, indexes, and unique constraints configured here

            modelBuilder.Entity<ModuleAccessControl>()
                .HasOne(m => m.Role)
                .WithMany()
                .HasForeignKey(m => m.RoleId);

            modelBuilder.Entity<UserRoleAssignment>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserRoleAssignment>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);


            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.NormalizedUserName).IsUnique();
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.NormalizedEmail);

            // ApplicationRole indexes
            modelBuilder.Entity<ApplicationRole>()
                .HasIndex(r => r.NormalizedName).IsUnique();

            // Module unique index
            modelBuilder.Entity<ModuleI>()
                .HasIndex(m => m.ModuleName).IsUnique();

            // Prevent duplicate access control entries
            modelBuilder.Entity<ModuleAccessControl>()
                .HasIndex(mac => new { mac.ModuleId, mac.RoleId })
                .IsUnique();

            // Prevent duplicate role assignments
            modelBuilder.Entity<UserRoleAssignment>()
                .HasIndex(ura => new { ura.UserId, ura.RoleId })
                .IsUnique();

            // Device ID unique constraint
            modelBuilder.Entity<TrustedDevice>()
                .HasIndex(td => td.DeviceIdentifier)
                .IsUnique();

            // Token/User combination index
            modelBuilder.Entity<TrustedDevice>()
                .HasIndex(td => new { td.UserId, td.DeviceIdentifier })
                .IsUnique(); // Allow same device to be trusted by different users

            // Identity relationships
            modelBuilder.Entity<UserRoleAssignment>()
                .HasOne(x => x.User)
                .WithMany(u => u.RoleAssignments)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserRoleAssignment>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<ModuleAccessControl>()
                .HasOne(x => x.Module)
                .WithMany(m => m.AccessControls)
                .HasForeignKey(x => x.ModuleId);

            modelBuilder.Entity<ModuleAccessControl>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<LoginAuditLog>()
                .Property(l => l.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<LoginAuditLog>()
                .HasIndex(l => l.UserId);

            modelBuilder.Entity<LoginAuditLog>()
                .HasIndex(l => l.LoginTime);

            modelBuilder.Entity<LoginAuditLog>()
                .HasIndex(l => new { l.UserId, l.Success, l.LoginTime });


            modelBuilder.Entity<LoginAuditLog>()
                .HasKey(l => l.Id);

            // Reset password workflow
            modelBuilder.Entity<PasswordResetToken>()
                .HasIndex(p => p.Token)
                .IsUnique();

            modelBuilder.Entity<PasswordResetToken>()
                .HasIndex(p => new { p.UserId, p.IsUsed });

            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmailAuditLog>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Recommended: don't cascade delete user emails

            modelBuilder.Entity<EmailAuditLog>()
                .HasIndex(e => e.ToEmail);

            modelBuilder.Entity<EmailAuditLog>()
                .HasIndex(e => e.TemplateType);

            modelBuilder.Entity<EmailAuditLog>()
                .HasIndex(e => e.SentAt);
        }

        /// <summary>
        /// Overrides the default SaveChangesAsync to prevent modifications or deletions of <see cref="LoginAuditLog"/> entries.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>The number of affected records.</returns>
        /// <exception cref="InvalidOperationException">Thrown if attempting to modify or delete a login audit log.</exception>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var modifiedLogs = ChangeTracker.Entries<LoginAuditLog>()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Deleted);

            if (modifiedLogs.Any())
            {
                throw new InvalidOperationException("LoginAuditLog entries are write-once and cannot be modified or deleted.");
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

}

