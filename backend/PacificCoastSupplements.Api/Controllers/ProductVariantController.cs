using Microsoft.AspNetCore.Mvc;
using PacificCoastSupplements.Api.DTOs;
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
        public async Task<IActionResult> Create(ProductVariantCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductVariantUpdateDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
