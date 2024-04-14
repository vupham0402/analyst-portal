using AnalystPortal.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AnalystPortal.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Sales> Sales { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationLogo> OrganizationLogos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var eyeprogpo = new Organization()
            {
                Id = 1,
                Name = "EyeProGPO",
                LogoUrl = "eyeprogpo.com"
            };
            builder.Entity<Organization>().HasData(eyeprogpo);
        }
    }
}
