using Microsoft.AspNetCore.Mvc;
using Portal.Application.DTO;
using Portal.Application.Interfaces;

namespace Portal.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendedoresController : ControllerBase
    {
        private readonly IVendedorAppService _service;

        public VendedoresController(IVendedorAppService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendedorReadDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VendedorReadDto>> Get(Guid id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<VendedorReadDto>> Create(VendedorCreateDto input)
        {
            var created = await _service.CreateAsync(input);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, VendedorUpdateDto input)
        {
            await _service.UpdateAsync(id, input);
            return NoContent();
        }

        //[HttpPut("{id:guid}/status")]
        //public async Task<IActionResult> UpdateStatus(Guid id)
        //{
        //    await _service.UpdateStatusAsync(id);
        //    return NoContent();
        //}

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}