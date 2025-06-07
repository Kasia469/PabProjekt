namespace Pab.WebAdmin.Models
{
    public class CreateOrderRequest
    {
        public string UserId { get; set; } = null!;
        public List<CreateOrderItem> Items { get; set; } = new();
    }

    public class CreateOrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
