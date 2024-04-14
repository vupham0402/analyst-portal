namespace AnalystPortal.API.Models.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public List<SalesDto> Sales { get; set; } = new List<SalesDto>();
    }
}
