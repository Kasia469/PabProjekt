using System;
using System.Collections.Generic;

namespace Pab.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        // 1. To już zapewne jest:
        public string UserId { get; set; } = null!;

        // 2. Dodajemy datę zamówienia:
        public DateTime OrderDate { get; set; }

        // 3. Dodajemy kwotę całkowitą:
        public decimal TotalAmount { get; set; }

        // 4. Lista pozycji:
        public List<OrderItem> Items { get; set; } = new();
    }
}
