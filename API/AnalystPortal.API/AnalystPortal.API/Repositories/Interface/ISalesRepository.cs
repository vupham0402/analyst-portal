using AnalystPortal.API.Models.Domain;

namespace AnalystPortal.API.Repositories.Interface
{
    public interface ISalesRepository
    {
        Task<Sales> CreateAsync(Sales sales);
        Task<IEnumerable<Sales>> GetAllAsync(int? queryOrg = null, int? queryCat = null, string? queryPro = null,
                                             string? sortBy = null, string? sortDirection = null,
                                             int? pageNumber = 1, int? pageSize = 50);

        Task<Sales?> GetByIdAsync(string id);
        Task<Sales?> UpdateAsync(Sales sales);
        Task<Sales?> DeleteAsync(string id);
        Task<int> GetCountAsync(int? queryOrg = null, int? queryCat = null, string? queryPro = null);
        Task<Dictionary<string, Dictionary<string, decimal>>> GetTotalSalesByOrgAndYearAsync(int? orgId = null, string? year = null);
        Task<Dictionary<string, decimal>> GetTotalSalesByRegionAsync(int? orgId = null, string? year = null);
        Task<Dictionary<string, Dictionary<string, decimal>>> GetTotalSalesByStateAndCityAsync(int? orgId = null, string? year = null);
        Task<Dictionary<string, Dictionary<string, decimal>>> GetTotalSalesByRegionAndCategoryAsync(int? orgId = null, string? year = null);
    }
}
