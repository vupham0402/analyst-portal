using System.ComponentModel.DataAnnotations;

namespace AnalystPortal.API.Models.Domain
{
    public class OrganizationLogo
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
    }
}
