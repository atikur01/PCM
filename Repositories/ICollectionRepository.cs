using PCM.Models;

namespace PCM.Repositories
{
    public interface ICollectionRepository
    {
        Task<Collection> GetByIdAsync(Guid id);
        Task<IEnumerable<Collection>> GetAllAsync();
        Task AddAsync(Collection collection);
        Task UpdateAsync(Collection collection);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Collection>> GetByUserId(Guid userId);
        Task<IEnumerable<Category>> GetAllCategoryAsync();
        
    }
}
