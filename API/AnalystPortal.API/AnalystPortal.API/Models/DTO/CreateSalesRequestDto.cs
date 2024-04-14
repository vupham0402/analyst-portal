namespace AnalystPortal.API.Models.DTO
{
    public class CreateSalesRequestDto
    {
        public string Id { get; set; }  
        public DateTime OrderDate { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public decimal Total { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
        public int[] Organizations { get; set; }
    }
}
