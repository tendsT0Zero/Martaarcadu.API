using Martaarcadu.Application.DTOs.SubscriptionPlan;
using Martaarcadu.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Martaarcadu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsPlanController : ControllerBase
    {
        private readonly ISubscriptionsPlanService _service;

        public SubscriptionsPlanController(ISubscriptionsPlanService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlanDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);

            if (result.IsSuccess)
            {

                return CreatedAtAction(nameof(GetById), new { id = result.ResponseObject }, result);
            }

            return BadRequest(result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlanDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, dto);

            if (!result.IsSuccess)
            {
                if (result.Message == "Plan not found.")
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                if (result.Message == "Plan not found.")
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}