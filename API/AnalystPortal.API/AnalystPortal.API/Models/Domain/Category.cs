using System.ComponentModel.DataAnnotations;

namespace AnalystPortal.API.Models.Domain
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public ICollection<Sales> Sales { get; set; }
    }
}
