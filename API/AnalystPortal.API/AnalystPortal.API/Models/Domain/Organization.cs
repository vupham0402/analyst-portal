using System.ComponentModel.DataAnnotations;

namespace AnalystPortal.API.Models.Domain
{
    public class Organization
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LogoUrl { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Sales> Sales { get; set; }
    }
}
