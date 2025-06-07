using Microsoft.EntityFrameworkCore;
using Pab.Domain.Entities;
using Pab.Domain.Interfaces;
using Pab.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pab.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            // Pobieramy zamówienia wraz ze wszystkimi pozycjami
            return await _context.Orders
                .Include(o => o.Items)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> AddAsync(Order order)
        {
            // Załóżmy, że w order.Items mamy wypełnione odpowiednio ProductId, Quantity, UnitPrice
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            // Pobranie istniejącego zamówienia wraz z pozycjami
            var existing = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (existing == null)
                return null;

            // Aktualizujemy nagłówek
            existing.UserId = order.UserId;
            existing.OrderDate = order.OrderDate;
            existing.TotalAmount = order.TotalAmount;

            // Usuń stare pozycje i dodaj nowe (najprostsze podejście)
            _context.OrderItems.RemoveRange(existing.Items);

            // Dodaj nowe pozycje (z dto albo z order.Items)
            existing.Items = order.Items;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Orders.FindAsync(id);
            if (existing == null)
                return false;

            _context.Orders.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
