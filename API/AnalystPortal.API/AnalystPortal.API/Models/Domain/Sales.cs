using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalystPortal.API.Models.Domain
{
    public class Sales
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public Category Categories { get; set; }

        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Region { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }
}
