using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Models.DTO;
using AnalystPortal.API.Repositories.Implementation;
using AnalystPortal.API.Repositories.Interface;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AnalystPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationRepository organizationRepository;
        private readonly ISalesRepository salesRepository;

        public OrganizationsController(IOrganizationRepository organizationRepository,
                                      ISalesRepository salesRepository)
        {
            this.organizationRepository = organizationRepository;
            this.salesRepository = salesRepository;
        }

        //Post: {apiBaseUrl}/api/organizations
        [HttpPost]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequestDto request)
        {
            // Convert DTO to Domain
            var org = new Organization
            {
                Name = request.Name,
                LogoUrl = request.LogoUrl
            };

            org = await organizationRepository.CreateAsync(org);

            // Convert Domain model back to DTO
            var response = new OrganizationDto
            {
                Id = org.Id,
                Name = org.Name,
                LogoUrl = org.LogoUrl
            };

            return Ok(response);
        }

        // Get: {apiBaseUrl}/api/organizations
        [HttpGet]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var org = await organizationRepository.GetAllAsync();

            // Convert Domain model to DTO
            var response = new List<OrganizationDto>();
            foreach (var eachOrg in org)
            {
                response.Add(new OrganizationDto
                {
                    Id = eachOrg.Id,
                    Name = eachOrg.Name,
                    LogoUrl = eachOrg.LogoUrl,
                    Sales = eachOrg.Sales.Select(x => new SalesDto
                    {
                        Id = x.Id,
                        OrderDate = x.OrderDate,
                        CategoryId = x.CategoryId,
                        ProductName = x.ProductName,
                        Total = x.Total,
                        City = x.City,
                        State = x.State,
                        Region = x.Region
                    }).ToList()
                });
            }

            return Ok(response);
        }

        //PUT: {apiBaseUrl}/api/organizations/{id}
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> UpdateOrganizationById([FromRoute] int id, UpdateOrganizationRequestDto request)
        {
            // Convert DTO to Domain model
            var org = new Organization
            {
                Id = id,
                Name = request.Name,
                LogoUrl = request.LogoUrl,
                Sales = new List<Sales>()
            };

            foreach (var eachSales in request.Sales)
            {
                var existingSales = await salesRepository.GetByIdAsync(eachSales);
                if (existingSales != null)
                {
                    org.Sales.Add(existingSales);
                }
            }

            // Call repository to update Organization Domain model
            var updatedOrg = await organizationRepository.UpdateAsync(org);
            if (updatedOrg == null)
            {
                return NotFound();
            }

            // Convert Domain model back to DTO
            var response = new OrganizationDto
            {
                Id = id,
                Name = request.Name,
                LogoUrl = request.LogoUrl,
                Sales = org.Sales.Select(x => new SalesDto
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate,
                    CategoryId = x.CategoryId,
                    ProductName = x.ProductName,
                    Total = x.Total,
                    City = x.City,
                    State = x.State,
                    Region = x.Region
                }).ToList()
            };

            return Ok(response);
        }

        //DELETE: {apiBaseUrl}/api/organizations/{id}
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> DeleteOrganization([FromRoute] int id)
        {
            var deletedOrg = await organizationRepository.DeleteAsync(id);
            if (deletedOrg == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new OrganizationDto
            {
                Id = id,
                Name = deletedOrg.Name,
                LogoUrl = deletedOrg.LogoUrl,
            };

            return Ok(response);
        }

        // Get: {apiBaseUrl}/api/organizations{id}
        [HttpGet]
        [Route("{id:int}")]
        //[Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetOrganizationsById([FromRoute] int id)
        {
            var org = await organizationRepository.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new OrganizationDto
            {
                Id = id,
                Name = org.Name,
                LogoUrl = org.LogoUrl,
                Sales = org.Sales.Select(x => new SalesDto
                {
                    Id = x.Id,
                    OrderDate = x.OrderDate,
                    CategoryId = x.CategoryId,
                    ProductName = x.ProductName,
                    Total = x.Total,
                    City = x.City,
                    State = x.State,
                    Region = x.Region
                }).ToList()
            };

            return Ok(response);
        }

        // Get: {apiBaseUrl}/api/organizations/{name}
        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetOrganiztionByName([FromRoute] string name)
        {
            var org = await organizationRepository.GetByNameAsync(name);
            if (org == null)
            {
                return NotFound();
            }
            Console.WriteLine(org.Name);
            // Convert Domain model to DTO
            var response = new OrganizationDto
            {
                Id = org.Id,
                Name = name,
                LogoUrl = org.LogoUrl
            };

            return Ok(response);
        }
    }
}
