using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Products.Queries.DeleteProduct
{
    //Recordar que el comando no devuelve nada, solo indica que se hizo la acción
    public record DeleteProductCommand(Guid Id) : IRequest;
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (product == null) 
                throw new Exception("Producto no encontrado");
            product.Desactivate(); // Cambia el estado a inactivo en lugar de eliminar

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
