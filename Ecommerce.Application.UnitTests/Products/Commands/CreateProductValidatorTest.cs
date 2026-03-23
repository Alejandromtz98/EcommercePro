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
using Ecommerce.Application.Features.Products.DTOs;
using FluentValidation;
using AutoMapper;

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
            
            _context.Products.Returns(new List<Product>().BuildMock());
            _context.Categories.Returns(new List<Category>().BuildMock());
        }
        [Fact]
        public async Task Validator_ShouldHaveError_whenPriceNegative()
        {
            // 1. Arrange: Creamos la lista
            var _categories = new List<Category>
            {
                new Category("Categoria de prueba", "Descripción de la categoría de prueba")
            };
            _context.Categories.Returns(_categories.BuildMock());

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
            result.Errors.Should().Contain(x => x.PropertyName == nameof(command.Price));     
        }
        [Fact]
        public async Task Validator_ShouldHaveError_whenCategoryDoesNotExist()
        {

            //Intentamos usar una categoría que no existe
            var command = new CreateProductCommand
            (
                "Producto valido",
                "Descripción del producto de prueba",
                10m,
                10,
                Guid.NewGuid() // Asumimos que esta categoría NO existe en el contexto simulado
            );
            //2. Act - Ejecutar la validación
            var result = await _validator.ValidateAsync(command);
            //3. Assert - Verificar que la validación falle y contenga el error esperado
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == nameof(command.CategoryId));
            result.Errors.Should().Contain(x => x.ErrorMessage.Contains("no existe"));
        }
    }
}
