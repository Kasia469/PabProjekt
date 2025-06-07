namespace Pab.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Klucz obcy do zamówienia
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // Id produktu (może być też nawigacja do Product)
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
