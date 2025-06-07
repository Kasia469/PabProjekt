using System.Collections.Generic;

namespace Pab.API.GraphQL
{
    public class AddOrderInput
    {
        public string UserId { get; set; } = null!;
        public List<OrderItemInput> Items { get; set; } = new();
    }
}
