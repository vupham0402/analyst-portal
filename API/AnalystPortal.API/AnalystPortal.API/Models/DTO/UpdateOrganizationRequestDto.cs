namespace AnalystPortal.API.Models.DTO
{
    public class UpdateOrganizationRequestDto
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public List<string> Sales { get; set; } = new List<string>();
    }
}
