using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.DTOs
{
    public record CreateProductDto(
         string Name,
         string Description,
         decimal Price,
         int Stock,
         Guid CategoryId);
}
