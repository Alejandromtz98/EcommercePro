using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Features.Products.Commands.UpdatrProduct;
using Ecommerce.Domain.Entitties;
using FluentAssertions;
using NSubstitute;
using MockQueryable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.Common.Exceptions;

namespace Ecommerce.Application.UnitTests.Products.Commands
{
    public class UpdateProductHandlerTests
    {
        private readonly IApplicationDbContext _context;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _context = Substitute.For<IApplicationDbContext>();
            _handler = new UpdateProductHandler(_context);
        }
        [Fact]
        public async Task Handle_ShouldUpdatProduct_WhenRequestIsValid()
        {
            //1. Preparar el escenario
            var productId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            // Simular que el producto existe en la base de datos
            var existingProduct = new
                Product(
                    "Producto de prueba",
                    "Descripción de prueba",
                    10m,
                    100,
                    categoryId
                );
            //Tip Senior: Forzar el ID  que queremos testear
            typeof(Product).GetProperty("Id").SetValue(existingProduct, productId);

            var productMock = new List<Product> { existingProduct }.BuildMock();
            _context.Products.Returns(productMock);

            var command = new UpdateProductCommand
            (
                productId,
                "Producto Actualizado",
                "Descripción actualizada",
                150m,
                10,
                categoryId
            );

            //2. Ejecutar la acción
            await _handler.Handle(command, CancellationToken.None);

            //3. Verificar el resultado
            existingProduct.Name.Should().Be(command.Name);
            existingProduct.Price.Should().Be(command.Price);
            await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            //Arrange
            var productsMock = new List<Product>().BuildMock();
            _context.Products.Returns(productsMock);

            var command = new UpdateProductCommand
            (
                Guid.NewGuid(),
                "Producto actualizado",
                "Descripción actualizada",
                150m,
                10,
                Guid.NewGuid()
            );

            //Verificar que el handler lance una excepción correcta cuando el producto no existe
            await _handler.Invoking(h => h.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<NotFoundException>();
        }
    }
}
