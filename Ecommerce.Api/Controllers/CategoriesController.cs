using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.Features.Categories.Commands.CreateCategory;
using Ecommerce.Application.Features.Categories.Queries.GetCategories;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetCategoriesQuery());
            return Ok(result); // Devuelve un resultado adecuado
        }
    }
}
