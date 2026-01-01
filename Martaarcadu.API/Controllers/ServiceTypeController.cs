using Martaarcadu.Application.DTOs.ServiceType;
using Martaarcadu.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Martaarcadu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Uncomment this later to protect these endpoints
    public class ServiceTypesController : ControllerBase
    {
        private readonly IServiceTypeService _service;

        public ServiceTypesController(IServiceTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceTypeDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateServiceTypeDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}