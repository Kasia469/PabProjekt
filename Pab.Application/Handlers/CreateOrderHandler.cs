using Pab.Application.Commands;
using Pab.Domain.Entities;
using Pab.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Pab.Application.Handlers
{
    public class CreateOrderHandler
    {
        private readonly IOrderRepository _repo;

        public CreateOrderHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        // Załóżmy, że CreateOrderCommand wygląda mniej więcej tak:
        // public class CreateOrderCommand { public string UserId; public Dictionary<int, int> Items; }
        public async Task Handle(CreateOrderCommand cmd)
        {
            var order = new Order
            {
                UserId = cmd.UserId,
                OrderDate = DateTime.UtcNow
            };

            foreach (var (productId, qty) in cmd.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = productId,
                    Quantity = qty,
                    // Zakładamy, że cenę pobierzesz później (może z bazy, albo przekażesz w cmd)
                    UnitPrice = 0m
                });
            }

            await _repo.AddAsync(order);
        }
    }
}
