using AnalystPortal.API.Models.Domain;

namespace AnalystPortal.API.Models.DTO
{
    public class SalesDto
    {
        public string Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public decimal Total { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
        public List<OrganizationDto> Organizations { get; set; } = new List<OrganizationDto>();
        public int Count { get; set; }
    }
}
