using AnalystPortal.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnalystPortal.API.Data
{
    public class AuthDbContext : IdentityDbContext<User>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var viewerRoleId = "dfaf4db5-30a1-4f26-8c14-1bf1e0068eb2";
            var editorRoleId = "6e867670-3acc-46cc-a04d-025d2ffc4bcb";

            // Create Member and Admin roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = viewerRoleId,
                    Name = "Viewer",
                    NormalizedName = "Viewer".ToUpper(),
                    ConcurrencyStamp = viewerRoleId
                },
                new IdentityRole()
                {
                    Id = editorRoleId,
                    Name = "Editor",
                    NormalizedName = "Editor".ToUpper(),
                    ConcurrencyStamp = editorRoleId
                }
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Create an Admin User
            var adminUserId = "c7ce436d-b1ad-4f5a-b6e5-47b785a8554d";
            var admin = new User()
            {
                Id = adminUserId,
                FirstName = "Admin",
                LastName = "User",
                OrganizationId = 1,
                UserName = "admin@analystportal.com",
                Email = "admin@analystportal.com",
                NormalizedEmail = "admin@analystportal.com".ToUpper(),
                NormalizedUserName = "admin@analystportal.com".ToUpper()
            };
            admin.PasswordHash = new PasswordHasher<User>().HashPassword(admin, "Admin@123");
            builder.Entity<User>().HasData(admin);

            // Give Roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = viewerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = editorRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
