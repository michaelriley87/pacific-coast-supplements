using Microsoft.AspNetCore.Mvc;
using PacificCoastSupplements.Api.DTOs;
using PacificCoastSupplements.Api.Exceptions;
using PacificCoastSupplements.Api.Services;

namespace PacificCoastSupplements.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantController : ControllerBase
    {
        private readonly ProductVariantService _service;

        public ProductVariantController(ProductVariantService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ProductVariantReadDto>> Create([FromBody] ProductVariantCreateDto dto)
        {
            if (dto.ProductId is null)
                throw new BadRequestException("ProductId is required when creating a variant directly.");

            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductVariantUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

    }
}
