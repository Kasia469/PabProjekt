using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Pab.Domain.Entities;
using Pab.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Pab.API.GraphQL
{
    [ExtendObjectType("Query")]
    public class Query
    {
        // bez [UseDbContext], zamiast tego w parametrze IDbContextFactory
        public IQueryable<Product> GetProducts(
            [Service] IDbContextFactory<AppDbContext> dbFactory)
        {
            var db = dbFactory.CreateDbContext();
            return db.Products;
        }

        public async Task<Product?> GetProductByIdAsync(int id, [Service] IDbContextFactory<AppDbContext> dbFactory)
        {
            var db = dbFactory.CreateDbContext();
            return await db.Products.FindAsync(id);
        }

    }
}
