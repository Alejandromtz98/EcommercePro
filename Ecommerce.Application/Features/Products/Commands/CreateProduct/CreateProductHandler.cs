using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entitties;
using Ecommerce.Application.Common.Interfaces;
using MediatR;
using FluentValidation;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler: IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IValidator<CreateProductCommand> _validator; // Inyecta el validador

        public CreateProductHandler(IApplicationDbContext context, IValidator<CreateProductCommand> validator)
        {
            _context = context;
            _validator = validator;
        }
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            /* Forma manual: 
             *  //1. Validar Manualmente
            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);
            //2. Si hay errores, lanzar el manejador de errores
            if (!validatorResult.IsValid)
            {
                //Excepcion simple (junior)
                var errors = string.Join(", ", validatorResult.Errors.Select(e => e.ErrorMessage));
                throw new Exception(errors);
            }
            */
            //Usando Pipeline de MediatR, la validación se ejecutará automáticamente antes de llegar a este punto, por lo que no es necesario validar manualmente aquí.
            // 1. Creamos la entidad usando nuestro constructor blindado
            var product = new Product(
                name: request.Name,
                description: request.Description,
                price: request.Price,
                stock: request.Stock,
                categoryId: request.CategoryId);

            //2. Persistencia
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            //3. Retornamos el Id del producto creado
            return product.Id;
        }
    }
}
