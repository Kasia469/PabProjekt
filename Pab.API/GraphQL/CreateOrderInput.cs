namespace Pab.API.GraphQL.Inputs
{
    public record OrderItemInput(int ProductId, int Quantity);
    public record CreateOrderInput(int UserId, List<OrderItemInput> Items);
}
