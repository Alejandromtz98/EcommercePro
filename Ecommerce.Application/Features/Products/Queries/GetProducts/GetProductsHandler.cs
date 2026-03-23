using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Models;
using Ecommerce.Application.Features.Products.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsQuery, PaginatedList<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private object _pageSize;

        public GetProductsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            // 1. Preparamos la consulta (IQueryable) sin ejecutarla aún
            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking(); // MEJORA: No rastrear cambios (más rápido para lectura)

            // 2. Filtro de búsqueda (Si el usuario escribió algo)
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term));
            }

            // 3. Proyección al DTO
            var projection = query.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                //p.Category.Name
                p.Category != null ? p.Category.Name : "Categoría desconocida"
            ));
            int _pageSize = (request.PageSize > 50) ? 50 : request.PageSize;
            // 4. Ejecución de la Paginación usando nuestra herramienta
            return await PaginatedList<ProductDto>.CreateAsync(
                projection,
                request.PageNumber,
                _pageSize);
            /*var items = await projection.ToListAsync(cancellationToken);
            return new PaginatedList<ProductDto>(items, request.PageNumber, items.Count, request.PageSize);*/
        }
    }
}
