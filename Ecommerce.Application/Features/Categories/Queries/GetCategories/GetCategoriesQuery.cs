using Ecommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Categories.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;

    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetCategoriesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Select(c => new CategoryDto(
                    c.Id, 
                    c.Name,
                    c.Description
                    ))
                .ToListAsync(cancellationToken);
        }
    }
}
