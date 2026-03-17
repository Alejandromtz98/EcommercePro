using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    //Un record es una clase inmutable que se utiliza para representar datos.
    //En este caso, CreateProductCommand es un record que representa el comando para crear un producto.
    //Implementa la interfaz IRequest<Guid> de MediatR,
    //lo que indica que este comando devolverá un Guid (el Id del producto creado) cuando se ejecuta
    public record CreateProductCommand(
            string Name,
            string Description,
            decimal Price,
            int Stock,
            Guid CategoryId): IRequest<Guid>; //Devuelve el Id del producto creado
}
