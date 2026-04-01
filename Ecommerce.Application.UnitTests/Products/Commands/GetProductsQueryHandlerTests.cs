using AutoMapper;
using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Features.Products.DTOs;
using Ecommerce.Application.Features.Products.Queries.GetProducts;
using Ecommerce.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.UnitTests.Products.Commands
{
    public class GetProductsQueryHandlerTests
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly GetProductsHandler _handler;

        public GetProductsQueryHandlerTests()
        {
            // 1. Mock del Contexto
            _context = Substitute.For<IApplicationDbContext>();

            // 2. Configuración Real de AutoMapper (Regla de oro: No mockear el Mapper en Queries)
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Product, ProductDto>()
                   .ConstructUsing(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.Stock, p.Category.Name));
            });
            _mapper = config.CreateMapper();

            // 3. Instanciamos el Handler
            _handler = new GetProductsHandler(_context);
        }
        [Fact]
        public async Task Handle_ShouldReturnCorrectPage_WhenMultipleProductsExist()
        {
            // ARRANGE: Crear datos ficticios
            var category = new Category("Categoria de prueba", "Descripción de la categoría de prueba");
            var products = new List<Product>();

            for (int i = 1; i <= 15; i++)
            {
                // Si el constructor es privado, esto seguirá fallando.
                // Debes usar el constructor que sea accesible (público o interno).
                var p = new Product(
                    $"Product {i}",
                    $"Description for product {i}",
                    100m,
                    100 + i,
                    category.Id
                );
                //p.Category = category; // Asignamos la categoría al producto
                products.Add(p);
            }

            // Convertimos la lista en un Mock de IQueryable asíncrono
            var mockDbSet = products.BuildMock();
            _context.Categories.Returns(new List<Category> { category }.BuildMock());
            _context.Products.AsNoTracking().Returns(mockDbSet);
            _context.Products.Returns(mockDbSet);

            // Pedimos la página 2 con 5 elementos
            var query = new GetProductsQuery(SearchTerm: null, PageNumber: 2, PageSize: 5);

            // ACT
            var result = await _handler.Handle(query, CancellationToken.None);

            // ASSERT
            result.Items.Should().HaveCount(5); // Deben venir 5
            result.TotalCount.Should().Be(15); // El total debe ser 15
            result.PageNumber.Should().Be(2);  // Debe decir que es la página 2
            result.TotalPages.Should().Be(3);  // 15 / 5 = 3 páginas
            result.HasNextPage.Should().BeTrue();
            result.HasPreviousPage.Should().BeTrue();
        }
    }
}
