using HotChocolate.Types;
using Pab.Domain.Entities;

namespace Pab.API.GraphQL
{
    public class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> d)
        {
            d.Field(p => p.Id).Type<NonNullType<IdType>>();
            d.Field(p => p.Name).Type<NonNullType<StringType>>();
            d.Field(p => p.Price).Type<NonNullType<DecimalType>>();
            d.Field(p => p.Stock).Type<NonNullType<IntType>>();
        }
    }
}
