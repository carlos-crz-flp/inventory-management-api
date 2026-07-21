using Asp.Versioning;
using Inventory.Application.Features.Products.CreateProduct;
using Inventory.Application.Features.Products.DecreaseStock;
using Inventory.Application.Features.Products.DeleteProduct;
using Inventory.Application.Features.Products.GetProductById;
using Inventory.Application.Features.Products.GetProducts;
using Inventory.Application.Features.Products.IncreaseStock;
using Inventory.Application.Features.Products.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetProductsQuery(),
                cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetProductByIdQuery(id),
                cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new
                {
                    version = HttpContext.GetRequestedApiVersion()!.ToString(),
                    id
                },
                null);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateProductCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeleteProductCommand(id),
                cancellationToken);

            return NoContent();
        }

        [HttpPost("{id:guid}/increase-stock")]
        public async Task<IActionResult> IncreaseStock(
            Guid id,
            IncreaseStockCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.ProductId)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("{id:guid}/decrease-stock")]
        public async Task<IActionResult> DecreaseStock(
            Guid id,
            DecreaseStockCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.ProductId)
                return BadRequest();

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}