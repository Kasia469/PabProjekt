using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Pab.API.Controllers;        
using Pab.WebAdmin.Models;
using Pab.Domain.Entities;
using Pab.Domain.Interfaces;

namespace Pab.Tests
{
    public class ProductsControllerUnitTests
    {
        [Fact]
        public async Task GetAll_Returns200AndExpectedProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Foo", Price = 10m },
                new Product { Id = 2, Name = "Bar", Price = 20m }
            };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(products);

            var controller = new ProductsController(repoMock.Object);

            var result = await controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(ok.Value);

            Assert.Collection(dtos,
                dto =>
                {
                    Assert.Equal(1, dto.Id);
                    Assert.Equal("Foo", dto.Name);
                    Assert.Equal(10m, dto.Price);
                },
                dto =>
                {
                    Assert.Equal(2, dto.Id);
                    Assert.Equal("Bar", dto.Name);
                    Assert.Equal(20m, dto.Price);
                });

            repoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
