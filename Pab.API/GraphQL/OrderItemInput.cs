namespace Pab.API.GraphQL
{
    public class OrderItemInput
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
