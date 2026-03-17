using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ecommerce.Application.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(
        string Name,
        string Description) : IRequest<Guid>;
    
}
