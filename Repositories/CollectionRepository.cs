using Microsoft.EntityFrameworkCore;
using PCM.Data;
using PCM.Models;

namespace PCM.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly AppDbContext _context;

        public CollectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Collection> GetByIdAsync(Guid id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return null;
            }
            else
            {
                return collection;
            }
        }

        public async Task<IEnumerable<Collection>> GetAllAsync()
        {
            return await _context.Collections.ToListAsync();
        }

        public async Task AddAsync(Collection collection)
        {
            await _context.Collections.AddAsync(collection);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Collection collection)
        {
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var collection = await GetByIdAsync(id);
            if (collection != null)
            {
                _context.Collections.Remove(collection);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Collection>> GetByUserId(Guid userId)
        {
            var collections = await _context.Collections
                .Where(c => c.UserId == userId)
                .ToListAsync();
            return collections;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }
    }
}
