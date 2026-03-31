using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.UpdatrProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity == null) 
                throw new NotFoundException($"No se encontró un producto con el ID {request.Id}.");

            entity.UpdateProduct(
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId
            );
            await _context.SaveChangesAsync(cancellationToken);
            
        }
    }
}
