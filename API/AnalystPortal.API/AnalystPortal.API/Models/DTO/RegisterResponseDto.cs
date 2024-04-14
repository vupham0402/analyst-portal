﻿namespace AnalystPortal.API.Models.DTO
{
    public class RegisterResponseDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int OrganizationId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
