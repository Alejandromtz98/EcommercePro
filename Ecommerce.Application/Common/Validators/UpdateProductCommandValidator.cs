using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Features.Products.Commands.UpdatrProduct;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del producto es requerido.")
                .NotNull().WithMessage("El nombre del producto es requerido.")
                .MaximumLength(100).WithMessage("El nombre del producto no puede exceder los 100 caracteres.")
                .MustAsync(BeUnique).WithMessage("El nombre '{PropertyValue}' ya existe, puede estar Inactivo o ya esta en uso por otro producto");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción del producto es requerida.")
                .NotNull().WithMessage("La descripción del producto es requerida.")
                .MaximumLength(500).WithMessage("La descripción del producto no puede exceder los 500 caracteres.");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio del producto debe ser mayor que cero.");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock del producto no puede ser negativo.");
            RuleFor(x => x.CategoryId)
                //.NotEmpty().WithMessage("La categoría del producto es requerida.");
                .NotEmpty()
                .MustAsync(async (id, cancellation) =>
                    await _context.Categories.AnyAsync(c => c.Id == id, cancellation))
                .WithMessage("La categoría especificada no existe.");
            //Regla asincrona
            /*RuleFor(x => x.Name)
                .MustAsync(async (name, cancellation) =>
                {
                    //Verificamos si ya existe el producto
                    var exist = await _context.Products
                    .IgnoreQueryFilters()
                    .AnyAsync(p => p.Name.ToLower() == name.ToLower(), cancellation);

                    return !exist; //Si exist es true, entonces el nombre ya existe y la validación falla, por eso retornamos !exist
                })
                .WithMessage("Ya existe un producto con el nombre '{PropertyValue}'. El nombre debe ser unico.");
            */
        }
        private async Task<bool> BeUnique(UpdateProductCommand model, string name, CancellationToken cancellationToken)
        {
            //Regla asincrona personalizada para verificar que el nombre del producto sea unico, incluso si el producto esta Inactivo.
            // .IgnoreQueryFilters() hace que EF ignore el "IsActive == true" 
            // y busque en absolutamente todos los registros.
            return !await _context.Products
                .IgnoreQueryFilters()
                .AnyAsync(p => p.Name.ToLower() == name.ToLower() && p.Id != model.Id, cancellationToken);
        }

    }
}
