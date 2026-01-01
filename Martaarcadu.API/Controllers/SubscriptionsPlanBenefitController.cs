using Martaarcadu.Application.DTOs.SubscriptionPlanBenefitsDtos;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Martaarcadu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanBenefitsController : ControllerBase
    {
        private readonly ISubscriptionsPlanBenefitService _service;

        public SubscriptionPlanBenefitsController(ISubscriptionsPlanBenefitService service)
        {
            _service = service;
        }

        [HttpGet("plan/{planId}")]
        public async Task<IActionResult> GetAllByPlanId(Guid planId)
        {
            var result = await _service.GetAllByPlanIdAsync(planId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("plan/{planId}")]
        public async Task<IActionResult> Create(Guid planId, [FromBody] CreatePlanBenefit dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _service.CreateAsync(planId, dto);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = result.ResponseObject }, result);

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlanBenefit dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

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