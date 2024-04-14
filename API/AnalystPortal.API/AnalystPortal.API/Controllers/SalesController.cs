using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Models.DTO;
using AnalystPortal.API.Repositories.Implementation;
using AnalystPortal.API.Repositories.Interface;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography;

namespace AnalystPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesRepository salesRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly ICategoryRepository categoryRepository;

        public SalesController(ISalesRepository salesRepository,
                               IOrganizationRepository organizationRepository,
                               ICategoryRepository categoryRepository)
        {
            this.salesRepository = salesRepository;
            this.organizationRepository = organizationRepository;
            this.categoryRepository = categoryRepository;
        }

        //Post: {apiBaseUrl}/api/sales
        [HttpPost]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> CreateSales([FromBody] CreateSalesRequestDto request)
        {
            // Convert DTO to Domain
            var sales = new Sales
            {
                Id = request.Id,
                OrderDate = request.OrderDate,
                CategoryId = request.CategoryId,
                ProductName = request.ProductName,
                Total = request.Total,
                City = request.City,
                State = request.State,
                Region = request.Region,
                Organizations = new List<Organization>()
            };

            foreach (var eachOrg in request.Organizations)
            {
                var existingOrg = await organizationRepository.GetByIdAsync(eachOrg);
                if (existingOrg is not null)
                {
                    sales.Organizations.Add(existingOrg);
                }
            }

            sales = await salesRepository.CreateAsync(sales);

            var category = await categoryRepository.GetByIdAsync(sales.CategoryId);

            // Convert Domain model back to DTO
            var response = new SalesDto
            {
                Id = sales.Id,
                OrderDate = sales.OrderDate,
                CategoryId = sales.CategoryId,
                CategoryName = category.CategoryName,
                ProductName = sales.ProductName,
                Total = sales.Total,
                City = sales.City,
                State = sales.State,
                Region = sales.Region,
                Organizations = sales.Organizations.Select(x => new OrganizationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    LogoUrl = x.LogoUrl,
                }).ToList()
            };

            return Ok(response);
        }

        // Get: {apiBaseUrl}/api/sales
        [HttpGet]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetAllSales([FromQuery] int? queryOrg, [FromQuery] int? queryCat, [FromQuery] string? queryPro,
                                                     [FromQuery] string? sortBy, [FromQuery] string? sortDirection,
                                                     [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var sales = await salesRepository.GetAllAsync(queryOrg, queryCat, queryPro, sortBy, sortDirection, pageNumber, pageSize);
            var count = await salesRepository.GetCountAsync(queryOrg, queryCat, queryPro);

            // Convert Domain model to DTO
            var response = new List<SalesDto>();
            foreach (var eachSales in sales)
            {
                var category = await categoryRepository.GetByIdAsync(eachSales.CategoryId);
                response.Add(new SalesDto
                {
                    Id = eachSales.Id,
                    OrderDate = eachSales.OrderDate,
                    CategoryId = eachSales.CategoryId,
                    CategoryName = category.CategoryName,
                    ProductName = eachSales.ProductName,
                    Total = eachSales.Total,
                    City = eachSales.City,
                    State = eachSales.State,
                    Region = eachSales.Region,
                    Organizations = eachSales.Organizations.Select(x => new OrganizationDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        LogoUrl = x.LogoUrl,
                    }).ToList(),
                    Count = count
                });
            }

            return Ok(response);
        }

        //PUT: {apiBaseUrl}/api/sales/{id}
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> UpdateSalesById([FromRoute] string id, UpdateSalesRequestDto request)
        {
            // Convert DTO to Domain model
            var sales = new Sales
            {
                Id = id,
                OrderDate = request.OrderDate,
                CategoryId = request.CategoryId,
                ProductName = request.ProductName,
                Total = request.Total,
                City = request.City,
                State = request.State,
                Region = request.Region,
                Organizations = new List<Organization>()
            };

            foreach (var eachOrg in request.Organizations)
            {
                var existingOrg = await organizationRepository.GetByIdAsync(eachOrg);
                if (existingOrg is not null)
                {
                    sales.Organizations.Add(existingOrg);
                }
            }

            var updatedSales = await salesRepository.UpdateAsync(sales);

            if (updatedSales == null)
            {
                return NotFound();
            }

            var category = await categoryRepository.GetByIdAsync(sales.CategoryId);

            // Convert Domain model back to DTO
            var response = new SalesDto
            {
                Id = sales.Id,
                OrderDate = sales.OrderDate,
                CategoryId = sales.CategoryId,
                CategoryName = category.CategoryName,
                ProductName = sales.ProductName,
                Total = sales.Total,
                City = sales.City,
                State = sales.State,
                Region = sales.Region,
                Organizations = sales.Organizations.Select(x => new OrganizationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    LogoUrl = x.LogoUrl,
                }).ToList()
            };

            return Ok(response);
        }

        //DELETE: {apiBaseUrl}/api/sales/{id}
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> SalesOrganization([FromRoute] string id)
        {
            var deletedSales = await salesRepository.DeleteAsync(id);
            if (deletedSales == null)
            {
                return NotFound();
            }

            var category = await categoryRepository.GetByIdAsync(deletedSales.CategoryId);

            // Convert Domain model back to DTO
            var response = new SalesDto
            {
                Id = deletedSales.Id,
                OrderDate = deletedSales.OrderDate,
                CategoryId = deletedSales.CategoryId,
                CategoryName = category.CategoryName,
                ProductName = deletedSales.ProductName,
                Total = deletedSales.Total,
                City = deletedSales.City,
                State = deletedSales.State,
                Region = deletedSales.Region,
            };

            return Ok(response);
        }

        // Get: {apiBaseUrl}/api/sales/{id}
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetSalesById([FromRoute] string id)
        {
            var sales = await salesRepository.GetByIdAsync(id);
            if (sales == null)
            {
                return NotFound();
            }

            var category = await categoryRepository.GetByIdAsync(sales.CategoryId);

            // Convert Domain model back to DTO
            var response = new SalesDto
            {
                Id = sales.Id,
                OrderDate = sales.OrderDate,
                CategoryId = sales.CategoryId,
                CategoryName = category.CategoryName,
                ProductName = sales.ProductName,
                Total = sales.Total,
                City = sales.City,
                State = sales.State,
                Region = sales.Region,
                Organizations = sales.Organizations.Select(x => new OrganizationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    LogoUrl = x.LogoUrl,
                }).ToList()
            };

            return Ok(response);
        }

        // Get: {apiBaseUrl}/api/sales/count
        [HttpGet]
        [Route("count")]
        //[Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetSalesTotal()
        {
            var count = await salesRepository.GetCountAsync();

            return Ok(count);
        }

        // Get: {apiBaseUrl}/api/sales/data1
        [HttpGet]
        [Route("data1")]
        [Authorize(Roles = "Viewer")]
        public async Task<IActionResult> GetTotalSalesByOrgAndYearAsync([FromQuery] int? orgId, [FromQuery] string? year)
        {
            try
            {
                var data = await salesRepository.GetTotalSalesByOrgAndYearAsync(orgId, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "Internal server error");
            }
        }

        // Get: {apiBaseUrl}/api/sales/data4
        [HttpGet]
        [Route("data4")]
        [Authorize(Roles = "Viewer")]
        public async Task<IActionResult> GetTotalSalesByRegionAndCategoryAsync([FromQuery] int? orgId, [FromQuery] string? year)
        {
            try
            {
                var data = await salesRepository.GetTotalSalesByRegionAndCategoryAsync(orgId, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "Internal server error");
            }
        }

        // Get: {apiBaseUrl}/api/sales/data2
        [HttpGet]
        [Route("data2")]
        [Authorize(Roles = "Viewer")]
        public async Task<IActionResult> GetTotalSalesByRegionAsync([FromQuery] int? orgId, [FromQuery] string? year)
        {
            try
            {
                var data = await salesRepository.GetTotalSalesByRegionAsync(orgId, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "Internal server error");
            }
        }

        // Get: {apiBaseUrl}/api/sales/data3
        [HttpGet]
        [Route("data3")]
        [Authorize(Roles = "Viewer")]
        public async Task<IActionResult> GetTotalSalesByStateAndCityAsync([FromQuery] int? orgId, [FromQuery] string? year)
        {
            try
            {
                var data = await salesRepository.GetTotalSalesByStateAndCityAsync(orgId, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
