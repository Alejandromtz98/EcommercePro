using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Features.Products.DTOs;
using MediatR;
using Ecommerce.Application.Features.Products.Queries.GetProducts;
using Ecommerce.Application.Features.Products.Queries.DeleteProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateStock;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using AutoMapper;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var command = _mapper.Map<CreateProductCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetProductsQuery());
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent(); // Devuelve un resultado adecuado para eliminación
        }
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(Guid id, UpdateProductStockCommand command)
        {
            if (id != command.Id)
                return BadRequest("El ID del producto no coincide");
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
