namespace Pab.API.Models
{
    // DTO używany przy tworzeniu nowego zamówienia
    public class CreateOrderDto
    {
        // Zakładam, że UserId to string (np. GUID) – dostosuj do swojego typu
        public string UserId { get; set; } = null!;

        // Lista par (ProductId, Quantity). W kluczu int–int: ProductId oraz Quantity
        public List<OrderItemCreateDto> Items { get; set; } = new List<OrderItemCreateDto>();
    }

    public class OrderItemCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
