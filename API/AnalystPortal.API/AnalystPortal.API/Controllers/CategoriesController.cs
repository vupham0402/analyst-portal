using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Models.DTO;
using AnalystPortal.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnalystPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ISalesRepository salesRepository;

        public CategoriesController(ICategoryRepository categoryRepository,
                                      ISalesRepository salesRepository)
        {
            this.categoryRepository = categoryRepository;
            this.salesRepository = salesRepository;
        }

        //Post: {apiBaseUrl}/api/organizations
        [HttpPost]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateCategoryRequestDto request)
        {
            // Convert DTO to Domain
            var cat = new Category
            {
                CategoryName = request.CategoryName
            };

            cat = await categoryRepository.CreateAsync(cat);

            // Convert Domain model back to DTO
            var response = new CategoryDto
            {
                Id = cat.Id,
                CategoryName = cat.CategoryName
            };

            return Ok(response);
        }

        // Get: {apiBaseUrl}/api/organizations
        [HttpGet]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetAllCategories()
        {
            var cat = await categoryRepository.GetAllAsync();

            // Convert Domain model to DTO
            var response = new List<CategoryDto>();
            foreach (var eachCat in cat)
            {
                response.Add(new CategoryDto
                {
                    Id = eachCat.Id,
                    CategoryName = eachCat.CategoryName,
                    Sales = eachCat.Sales.Select(x => new SalesDto
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
        public async Task<IActionResult> UpdateCategoryById([FromRoute] int id, UpdateCategoryRequestDto request)
        {
            // Convert DTO to Domain model
            var cat = new Category
            {
                Id = id,
                CategoryName = request.CategoryName,
                Sales = new List<Sales>()
            };

            foreach (var eachSales in request.Sales)
            {
                var existingSales = await salesRepository.GetByIdAsync(eachSales);
                if (existingSales != null)
                {
                    cat.Sales.Add(existingSales);
                }
            }

            // Call repository to update Organization Domain model
            var updatedCat = await categoryRepository.UpdateAsync(cat);
            if (updatedCat == null)
            {
                return NotFound();
            }

            // Convert Domain model back to DTO
            var response = new CategoryDto
            {
                Id = id,
                CategoryName = request.CategoryName,
                Sales = cat.Sales.Select(x => new SalesDto
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
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var deletedCat = await categoryRepository.DeleteAsync(id);
            if (deletedCat == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new CategoryDto
            {
                Id = id,
                CategoryName = deletedCat.CategoryName
            };

            return Ok(response);
        }

        // Get: {apiBaseUrl}/api/organizations{id}
        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var cat = await categoryRepository.GetByIdAsync(id);
            if (cat == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new CategoryDto
            {
                Id = id,
                CategoryName = cat.CategoryName,
                Sales = cat.Sales.Select(x => new SalesDto
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
    }
}
