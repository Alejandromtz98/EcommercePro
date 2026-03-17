using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Features.Products.DTOs;
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
        private readonly IMapper _mapper;

        public GetProductsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking() // Tip: Si solo vas a leer datos, AsNoTracking mejora el rendimiento al no rastrear cambios.
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider) // Proyecta directamente a ProductDto usando AutoMapper
                .ToListAsync(cancellationToken); // Ejecuta la consulta y devuelve la lista de ProductDto
        }
    }
}
