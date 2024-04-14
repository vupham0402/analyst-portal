using AnalystPortal.API.Data;
using AnalystPortal.API.Models.Domain;
using AnalystPortal.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;

namespace AnalystPortal.API.Repositories.Implementation
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SalesRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Sales> CreateAsync(Sales sales)
        {
            await dbContext.Sales.AddAsync(sales);
            await dbContext.SaveChangesAsync();
            return sales;
        }

        public async Task<IEnumerable<Sales>> GetAllAsync(int? queryOrg, int? queryCat, string? queryPro,
                                                          string? sortBy, string? sortDirection,
                                                          int? pageNumber = 1, int? pageSize = 50)
        {
            // Query
            var sales = dbContext.Sales.Include(x => x.Organizations).AsQueryable();

            // Filtering
            // Query Organization
            if (queryOrg.HasValue)
            {
                sales = sales.Where(x => x.Organizations.Any(o => o.Id == queryOrg));
            }
            // Query Catgory
            if (queryCat.HasValue)
            {
                sales = sales.Where(x => x.CategoryId == queryCat);
            }
            // Query Product Name
            if (!string.IsNullOrWhiteSpace(queryPro))
            {
                sales = sales.Where(x => x.ProductName.Contains(queryPro));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (string.Equals(sortBy, "Total", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;
                    sales = isAsc ? sales.OrderBy(x => x.Total) : sales.OrderByDescending(x => x.Total);
                }
            }

            // Pagination
            var skipResult = (pageNumber - 1) * pageSize;
            sales = sales.Skip(skipResult ?? 0).Take(pageSize ?? 50);

            return await sales.ToListAsync();
        }

        public async Task<Sales?> GetByIdAsync(string id)
        {
            return await dbContext.Sales.Include(x => x.Organizations).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Sales?> UpdateAsync(Sales sales)
        {
            var existingSales = await dbContext.Sales.Include(x => x.Organizations).FirstOrDefaultAsync(x => x.Id == sales.Id);
            if (existingSales == null)
            {
                return null;
            }

            // Update Sales
            dbContext.Entry(existingSales).CurrentValues.SetValues(sales);

            // Update Organizations
            existingSales.Organizations = sales.Organizations;

            await dbContext.SaveChangesAsync();

            return sales;
        }

        public async Task<Sales?> DeleteAsync(string id)
        {
            var existingSales = await dbContext.Sales.FirstOrDefaultAsync(x => x.Id == id);
            if (existingSales != null)
            {
                dbContext.Sales.Remove(existingSales);
                await dbContext.SaveChangesAsync();
                return existingSales;
            }

            return null;
        }

        public async Task<int> GetCountAsync(int? queryOrg = null, int? queryCat = null, string? queryPro = null)
        {
            // Query
            var sales = dbContext.Sales.Include(x => x.Organizations).AsQueryable();

            // Filtering
            // Query Organization
            if (queryOrg.HasValue)
            {
                sales = sales.Where(x => x.Organizations.Any(o => o.Id == queryOrg));
            }
            // Query Catgory
            if (queryCat.HasValue)
            {
                sales = sales.Where(x => x.CategoryId == queryCat);
            }
            // Query Product Name
            if (!string.IsNullOrWhiteSpace(queryPro))
            {
                sales = sales.Where(x => x.ProductName.Contains(queryPro));
            }
            
            return await sales.CountAsync();
        }

        public async Task<Dictionary<string, Dictionary<string, decimal>>> GetTotalSalesByOrgAndYearAsync(int? orgId = null, string? year = null)
        {
            if (year is not null)
            {
                // Only fetch data for the specified year, and group by month
                var salesData = await dbContext.Sales
                .Include(s => s.Organizations) // Ensure organizations are included
                .Where(s => s.Organizations.Any(o => o.Name != "EyeProGPO") && (s.OrderDate.Year == int.Parse(year)))
                .SelectMany(s => s.Organizations.Where(o => o.Id == orgId), (sale, organization) => new
                {
                    Month = sale.OrderDate.Month,
                    sale.Total
                })
                .ToListAsync();

                var monthlyTotals = salesData
                .GroupBy(s => s.Month)
                .OrderBy(group => group.Key)
                .ToDictionary(
                    monthGroup => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthGroup.Key),
                    monthGroup => monthGroup.Sum(s => s.Total)
                );

                return new Dictionary<string, Dictionary<string, decimal>> { { year, monthlyTotals } };
            }
            else
            {
                var salesData = await dbContext.Sales
                .Include(s => s.Organizations) // Ensure organizations are included
                .Where(s => s.Organizations.Any(o => o.Name != "EyeProGPO"))
                .SelectMany(s => s.Organizations.Where(o => o.Id == orgId), (sale, organization) => new
                {
                    Year = sale.OrderDate.Year,
                    Month = sale.OrderDate.Month,
                    sale.Total
                })
                .ToListAsync();

                var groupedData = salesData
                .GroupBy(s => s.Year)
                .ToDictionary(
                    yearGroup => yearGroup.Key.ToString(),
                    yearGroup =>
                    {
                        var monthlyTotals = yearGroup
                            .GroupBy(sale => sale.Month)
                            .OrderBy(group => group.Key)
                            .ToDictionary(
                                monthGroup => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthGroup.Key),
                                monthGroup => monthGroup.Sum(sale => sale.Total)
                            );

                        // Adding the "Total" key for yearly total
                        monthlyTotals.Add("Total", yearGroup.Sum(sale => sale.Total));

                        return monthlyTotals;
                    }
                );

                return groupedData;
            }
        }

        public async Task<Dictionary<string, Dictionary<string, decimal>>> GetTotalSalesByRegionAndCategoryAsync(int? orgId = null, string? year = null)
        {
            if (year is not null)
            {
                var salesData = await dbContext.Sales
                .Include(s => s.Organizations) // Ensure organizations are included
                .Where(s => s.Organizations.Any(o => o.Name != "EyeProGPO") && (s.OrderDate.Year == int.Parse(year)))
                .SelectMany(s => s.Organizations.Where(o => o.Id == orgId), (sale, organization) => new
                {
                    sale.Region,
                    sale.Categories.CategoryName,
                    sale.Total
                })
                .ToListAsync();

                var groupedData = salesData
                .GroupBy(s => s.Region)
                .OrderBy(group => group.Key)
                .ToDictionary(
                    regionGroup => regionGroup.Key,
                    regionGroup =>
                    {
                        var categoryTotals = regionGroup
                            .GroupBy(sale => sale.CategoryName)
                            .OrderBy(group => group.Key)
                            .ToDictionary(
                                categoryGroup => categoryGroup.Key,
                                categoryGroup => categoryGroup.Sum(sale => sale.Total)
                            );

                        // Adding the "Total" key for state total
                        //cityTotals.Add("Total", stateGroup.Sum(sale => sale.Total));

                        return categoryTotals;
                    }
                );

                return groupedData;
            }
            else
            {
                return null;
            }
        }

        public async Task<Dictionary<string, decimal>> GetTotalSalesByRegionAsync(int? orgId = null, string? year = null)
        {
            if (year is not null)
            {
                var salesData = await dbContext.Sales
                .Include(s => s.Organizations) // Ensure organizations are included
                .Where(s => s.Organizations.Any(o => o.Name != "EyeProGPO") && (s.OrderDate.Year == int.Parse(year)))
                .SelectMany(s => s.Organizations.Where(o => o.Id == orgId), (sale, organization) => new
                {
                    sale.Region,
                    sale.Total
                })
                .ToListAsync();

                var regionTotals = salesData
                .GroupBy(s => s.Region)
                .OrderBy(group => group.Key)
                .ToDictionary(
                    region => region.Key.ToString(),
                    region => region.Sum(s => s.Total)
                );

                return regionTotals;
            }
            else
            {
                return null;
            }
        }

        public async Task<Dictionary<string, Dictionary<string, decimal>>> GetTotalSalesByStateAndCityAsync(int? orgId = null, string? year = null)
        {
            if (year is not null)
            {
                var salesData = await dbContext.Sales
                .Include(s => s.Organizations) // Ensure organizations are included
                .Where(s => s.Organizations.Any(o => o.Name != "EyeProGPO") && (s.OrderDate.Year == int.Parse(year)))
                .SelectMany(s => s.Organizations.Where(o => o.Id == orgId), (sale, organization) => new
                {
                    sale.State,
                    sale.City,
                    sale.Total
                })
                .ToListAsync();

                var groupedData = salesData
                .GroupBy(s => s.State)
                .OrderBy(group => group.Key)
                .ToDictionary(
                    stateGroup => stateGroup.Key,
                    stateGroup =>
                    {
                        var cityTotals = stateGroup
                            .GroupBy(sale => sale.City)
                            .OrderBy(group => group.Key)
                            .ToDictionary(
                                cityGroup => cityGroup.Key,
                                cityGroup => cityGroup.Sum(sale => sale.Total)
                            );

                        // Adding the "Total" key for state total
                        cityTotals.Add("Total", stateGroup.Sum(sale => sale.Total));

                        return cityTotals;
                    }
                );

                return groupedData;
            }
            else
            {
                return null;
            }
        }
    }
}
