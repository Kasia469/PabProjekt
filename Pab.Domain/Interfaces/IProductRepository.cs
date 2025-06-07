using Pab.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pab.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);    // zwraca zaktualizowany produkt albo null, jeśli nie znaleziono
        Task<bool> DeleteAsync(int id);                  // zwraca true, jeśli usunięto, false jeśli nie znaleziono
    }
}
