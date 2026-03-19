using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Validationes;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using NSubstitute;
using Xunit;
using FluentAssertions;
using MockQueryable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entitties;

namespace Ecommerce.Application.UnitTests.Products.Commands
{
    public class CreateProductValidatorTest
    {
        private readonly IApplicationDbContext _context;
        private readonly CreateProductCommandValidator _validator;

        public CreateProductValidatorTest()
        {
            //Simulacion del contexto de la base de datos utilizando NSubstitute
            _context = Substitute.For<IApplicationDbContext>();
            _validator = new CreateProductCommandValidator(_context);
        }
        [Fact]
        public async Task Validator_ShouldHaveError_whenPriceNegative()
        {
            // 1. Arrange: Creamos la lista
            var categories = new List<Category>
            {
                new Category("Categoria de prueba", "Descripción")
            };

            // 2. FORZAMOS el Mock (Usa BuildMock() que es el estándar de la v9.0.0 que tienes)
            // Importante: Asegúrate de tener: using MockQueryable.NSubstitute;
            var categoriesMock = categories.BuildMock();
            var productsMock = new List<Product>().BuildMock();

            // 3. Configuramos el substituto
            _context.Categories.Returns(categoriesMock);
            _context.Products.Returns(productsMock);

            //1. Arrange - Configurar el comando con un precio negativo
            var command = new CreateProductCommand
            (
                "Producto de prueba",
                "Descripción del producto de prueba",
                -10m, // Precio negativo para probar la validación
                10,
                Guid.NewGuid() // Asumimos que esta categoría existe en el contexto simulado
            );

            //2. Act - Ejecutar la validación
            var result = await _validator.ValidateAsync(command);

            //3. Assert - Verificar que la validación falle y contenga el error esperado
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(command.Price));        }
    }
}
