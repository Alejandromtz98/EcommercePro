using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Features.Products.Queries.GetProducts
{
    //El query: no necesitas parametros
    public record GetProductsQuery : IRequest<IEnumerable<ProductDto>>;
    //El handler
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Category) //Join con categorias
                .Select(p => new ProductDto(
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Price,
                        p.Stock,
                        p.Category.Name //Proyeccion manual al DTO
                    ))
                .ToListAsync(cancellationToken);
        }
    }
}
