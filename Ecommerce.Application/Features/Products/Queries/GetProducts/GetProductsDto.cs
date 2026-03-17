using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Queries.GetProducts
{
    public record ProductDto(
            Guid Id,
            string Name,
            string Description,
            decimal Price,
            int Stock,
            string CategoryName // Incluimos el nombre de la categoría para facilitar la visualización en la respuesta,
                                // aunque no es parte de la entidad Product
        );
}
