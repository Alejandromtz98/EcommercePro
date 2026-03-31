using AutoMapper;
using Ecommerce.Application.Common.Models;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateStock;
using Ecommerce.Application.Features.Products.Commands.UpdatrProduct;
using Ecommerce.Application.Features.Products.DTOs;
using Ecommerce.Application.Features.Products.Queries.DeleteProduct;
using Ecommerce.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public ProductsController(IMediator mediator, IMapper mapper, ISender sender)
        {
            _mediator = mediator;
            _mapper = mapper;
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var command = _mapper.Map<CreateProductCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetProductsQuery());
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaginatedList<ProductDto>>> GetProducts(
        [FromQuery] GetProductsQuery query)
        {
            // El mediador se encarga de buscar el Handler que ya testeamos
            var result = await _sender.Send(query);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
        {
            if (id != command.Id) return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            await _mediator.Send(command);
            return NoContent();
        }
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(Guid id, UpdateProductStockCommand command)
        {
            if (id != command.Id)
                return BadRequest("El ID del producto no coincide");
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] //Exito
        [ProducesResponseType(StatusCodes.Status404NotFound)] //No existe el producto
        [ProducesResponseType(StatusCodes.Status400BadRequest)] //Error en la solicitud
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
    }
}
