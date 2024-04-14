using AnalystPortal.API.Models.Domain;

namespace AnalystPortal.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> UpdateAsync(Category category);
        Task<Category?> DeleteAsync(int id);
    }
}
