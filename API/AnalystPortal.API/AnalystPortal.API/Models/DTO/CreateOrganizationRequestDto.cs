using AnalystPortal.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace AnalystPortal.API.Models.DTO
{
    public class CreateOrganizationRequestDto
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
    }
}
