using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using MediatR;
using Ecommerce.Application.Features.Products.Queries.GetProducts;
using Ecommerce.Application.Features.Products.Queries.DeleteProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateStock;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
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
