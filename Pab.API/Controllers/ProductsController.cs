using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pab.Domain.Entities;
using Pab.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pab.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]   // tylko Admin może edytować/usunąć
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        // 1) GET /api/Products
        [HttpGet]
        [AllowAnonymous]   // wszyscy mogą pobierać listę
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _repo.GetAllAsync();
            return Ok(products);
        }

        // 2) GET /api/Products/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // 3) POST /api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            var created = await _repo.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // 4) PUT /api/Products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(int id, Product product)
        {
            if (id != product.Id) return BadRequest("ID w URL i w modelu nie są zgodne.");

            var updated = await _repo.UpdateAsync(product);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // 5) DELETE /api/Products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var removed = await _repo.DeleteAsync(id);
            if (!removed) return NotFound();
            return NoContent();
        }
    }
}
