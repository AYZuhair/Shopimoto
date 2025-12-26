using Microsoft.EntityFrameworkCore;
using Shopimoto.Domain.Entities;
using Shopimoto.Domain.Interfaces;
using Shopimoto.Infrastructure.Data;

namespace Shopimoto.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ShopimotoDbContext _context;

        public CategoryRepository(ShopimotoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
