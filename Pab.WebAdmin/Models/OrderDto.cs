namespace Pab.WebAdmin.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.UnitPrice * i.Quantity);
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
