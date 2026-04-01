using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;
using Ecommerce.Application.Common.Interfaces;
using MediatR;
using FluentValidation;
using AutoMapper;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler: IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IValidator<CreateProductCommand> _validator; // Inyecta el validador
        private readonly IMapper _mapper; // Inyecta AutoMapper

        public CreateProductHandler(IApplicationDbContext context, IValidator<CreateProductCommand> validator, IMapper mapper )
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
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
           /* var product = new Product(
                name: request.Name,
                description: request.Description,
                price: request.Price,
                stock: request.Stock,
                categoryId: request.CategoryId);
           */
           var product = _mapper.Map<Product>(request); // Usamos AutoMapper para mapear el comando a la entidad Product
            //2. Persistencia
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            //3. Retornamos el Id del producto creado
            return product.Id;
        }
    }
}
