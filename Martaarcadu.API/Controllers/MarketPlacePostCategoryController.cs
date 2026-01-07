using Martaarcadu.Application.DTOs.MarketPlacePostCategory;
using Martaarcadu.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Martaarcadu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketPlacePostCategoryController : ControllerBase
    {
        private readonly IMarketPlacePostCategoryService _service;

        public MarketPlacePostCategoryController(IMarketPlacePostCategoryService service)
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
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMarketPlacePostCategory dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

       
             var result = await _service.CreateAsync(dto);
             return CreatedAtAction(nameof(GetById), new { id = result.ResponseObject }, result);
                
                
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMarketPlacePostCategory dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


                var result = await _service.UpdateAsync(id, dto);
                if(result.ResponseObject != null)return Ok(result);
                return BadRequest(result);
            

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isDeleted = await _service.DeleteAsync(id);
            if (isDeleted.IsSuccess) return Ok(isDeleted);
            return BadRequest(isDeleted);
        }
    }
}
