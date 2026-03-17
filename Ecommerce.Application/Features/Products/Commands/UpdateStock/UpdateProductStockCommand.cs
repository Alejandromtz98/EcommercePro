using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Commands.UpdateStock
{
    public record UpdateProductStockCommand(Guid Id, int Quantity, bool IsAddition) : IRequest;

    public class UpdateProductStockHandler : IRequestHandler<UpdateProductStockCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateProductStockHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            //1.Buscamos el producto 
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
                throw new NotFoundException(nameof(product), request.Id);

            //Trampa de tiempo para simular una operación larga y probar el control de concurrencia optimista
            //await Task.Delay(10000, cancellationToken);
            //-----------------------------

            //2.Logica del dominio
            if (request.IsAddition)
                product.AddStock(request.Quantity);
            else
                //Aqui se dispara la InvalidOperation si no hay stock
                product.ReduceStock(request.Quantity);

            //3. Persistencia de datos
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
