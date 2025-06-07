using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pab.API.Models;
using Pab.Domain.Entities;
using Pab.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pab.API.Controllers
{
    [ApiController]
    [Authorize] // zakładając, że chcesz, by tylko uwierzytelnieni mogli operować zamówieniami
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;

        public OrdersController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        // GET: api/Orders
        [HttpGet]
        [AllowAnonymous] // lub usuń, jeśli chcesz wymagać autoryzacji
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepo.GetAllAsync();

            // Mapowanie na OrderDto:
            var dtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/Orders/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            var dto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return Ok(dto);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Tworzymy encję Order na podstawie DTO
            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0m, // Obliczymy poniżej
                Items = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = 0m // Załóżmy, że klient wpisał tylko ilość; cenę pobierzemy w repo (ew. z bazy)
                }).ToList()
            };

            // Jeśli chcesz pobrać unitPrice z bazy (np. z Product), musiałabyś tu dorzucić dodatkowe zapytanie.
            // Na początku ustawiamy 0 i później, po dodaniu do bazy, można je poprawić w warstwie domenowej / repozytorium.

            // Zapis do bazy
            var created = await _orderRepo.AddAsync(order);

            // Mapujemy z powrotem na OrderDto, aby klient dostał pełne dane (razem z wygenerowanym Id, cenami itd.)
            var resultDto = new OrderDto
            {
                Id = created.Id,
                UserId = created.UserId,
                OrderDate = created.OrderDate,
                TotalAmount = created.TotalAmount,
                Items = created.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        // PUT: api/Orders/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _orderRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            // Nadpisujemy tylko te pola, które chcemy aktualizować:
            existing.UserId = dto.UserId;
            existing.OrderDate = dto.OrderDate;
            existing.TotalAmount = dto.TotalAmount;

            // Jeśli w przyszłości chcesz aktualizować listę Items, tutaj trzeba dodać logikę.
            // Na razie przyjmujemy, że nie edytujemy listy pozycji.

            var updated = await _orderRepo.UpdateAsync(existing);
            if (updated == null)
                return NotFound(); // np. ktoś usunął w międzyczasie

            var resultDto = new OrderDto
            {
                Id = updated.Id,
                UserId = updated.UserId,
                OrderDate = updated.OrderDate,
                TotalAmount = updated.TotalAmount,
                Items = updated.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return Ok(resultDto);
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _orderRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var deleted = await _orderRepo.DeleteAsync(id);
            if (!deleted)
                return StatusCode(500, "Nie udało się usunąć zamówienia.");

            return NoContent();
        }
    }
}
