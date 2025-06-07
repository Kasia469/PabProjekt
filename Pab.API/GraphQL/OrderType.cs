using HotChocolate.Types;
using Pab.Domain.Entities;

namespace Pab.API.GraphQL
{
    public class OrderType : ObjectType<Order>
    {
        protected override void Configure(IObjectTypeDescriptor<Order> d)
        {
            d.Field(o => o.Id).Type<NonNullType<IdType>>();
            d.Field(o => o.UserId).Type<NonNullType<StringType>>();
            d.Field(o => o.OrderDate).Type<NonNullType<DateTimeType>>();
            d.Field(o => o.Items).Type<ListType<NonNullType<ObjectType<OrderItem>>>>();
        }
    }
}
