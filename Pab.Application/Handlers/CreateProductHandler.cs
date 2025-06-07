using Pab.Application.Commands;
using Pab.Domain.Entities;
using Pab.Domain.Interfaces;
using System.Threading.Tasks;

namespace Pab.Application.Handlers
{
    public class CreateProductHandler
    {
        private readonly IProductRepository _repo;
        public CreateProductHandler(IProductRepository repo) => _repo = repo;

        public async Task Handle(CreateProductCommand cmd)
        {
            var p = new Product
            {
                Name = cmd.Name,
                Price = cmd.Price,
                Stock = cmd.Stock
            };
            await _repo.AddAsync(p);
        }
    }
}
