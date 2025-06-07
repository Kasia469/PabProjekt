using Microsoft.EntityFrameworkCore;
using Pab.Domain.Entities;
using Pab.Domain.Interfaces;
using Pab.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pab.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existing = await _db.Products.FindAsync(product.Id);
            if (existing == null) return null;

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            await _db.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Products.FindAsync(id);
            if (existing == null) return false;

            _db.Products.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
