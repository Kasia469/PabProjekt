using System;
using System.Threading.Tasks;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Pab.Domain.Entities;
using Pab.Infrastructure.Data;

namespace Pab.API.GraphQL
{
    [ExtendObjectType("Mutation")]
    public class Mutation
    {
        public async Task<Product> AddProductAsync(
            AddProductInput input,
            [Service] IDbContextFactory<AppDbContext> dbFactory)
        {
            var db = dbFactory.CreateDbContext();
            var p = new Product { Name = input.Name, Price = input.Price };
            db.Products.Add(p);
            await db.SaveChangesAsync();
            return p;
        }

        // analogicznie Update i Delete…
    }
}
