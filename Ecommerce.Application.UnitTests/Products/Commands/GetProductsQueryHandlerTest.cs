using AutoMapper;
using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Validationes;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Queries.GetProducts;
using Ecommerce.Domain.Entitties;
using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.UnitTests.Products.Commands
{
    public class GetProductsQueryHandlerTest
    {
        private readonly IApplicationDbContext _context;
        private readonly CreateProductCommandValidator _validator;
        private readonly IMapper _mapper;
        public GetProductsQueryHandlerTest()
        {
            _context = Substitute.For<IApplicationDbContext>();
            _validator = new CreateProductCommandValidator(_context);
            _mapper = Substitute.For<IMapper>();
        }
        [Fact]
        public async Task Handle_ShouldReturnCorrectPageSize()
        {
            //1. Arrange: Creamos 15 productos de prueba
            var products = Enumerable.Range(1, 15)
            .Select(i => new Product { Id = Guid.NewGuid(), Name = $"Product {i}", Price = i * 10 })
            .ToList();

            var productsMock = products.AsQueryable().BuildMock();
            _context.Products.Returns(productsMock);

            var query = new GetProductsQuery(null, PageNumber: 2 , pageSize: 5);
            var handler = new GetProductsQueryHandler(_context, _mapper);

            //2. Act: Creamos el handler y ejecutamos la consulta con pageSize = 5
            var result = await handler.Handle(query, CancellationToken.None);

            //3.Assert
            result.Items.Count.Should().Be(5); // Verificamos que se devuelven 5 productos
            result.TotalCount.Should().Be(15); // Verificamos que el total de productos es 15
            result.TotalPages.Should().Be(3); // Verificamos que el total de páginas es 3 (15 productos / 5 por página)
            result.HasNextPage.Should().BeTrue(); // Verificamos que hay una página siguiente
        }
    }
}
