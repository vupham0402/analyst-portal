namespace AnalystPortal.API.Models.DTO
{
    public class UpdateCategoryRequestDto
    { 
        public string CategoryName { get; set; }
        public List<string> Sales { get; set; } = new List<string>();
    }
}
