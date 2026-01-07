using Martaarcadu.Application.DTOs.MarketPlacePost;
using Martaarcadu.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Martaarcadu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Requires JWT Token for all endpoints
    public class MarketPlacePostController : ControllerBase
    {
        private readonly IMarketPlacePostService _postService;

        public MarketPlacePostController(IMarketPlacePostService postService)
        {
            _postService = postService;
        }

        // Helper to get current User ID from JWT
        private Guid GetCurrentUserId()
        {
            
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(idClaim))
            {
                return Guid.Parse(idClaim);
            }

            // --- TESTING MODE ---
            return Guid.Parse("F24B21CB-6A92-4173-4C4B-08DE4C2440F0");
        }

        [HttpGet]
        //[AllowAnonymous] // Optional: Decide if public can see posts
        public async Task<IActionResult> GetAll()
        {
            var result = await _postService.GetAllAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _postService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpGet("my-posts")]
        public async Task<IActionResult> GetMyPosts()
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _postService.GetMyPostsAsync(userId);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateMarketPlacePostDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = GetCurrentUserId();
                var result = await _postService.CreateAsync(userId, dto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMarketPlacePostDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var userId = GetCurrentUserId();
                var result = await _postService.UpdateAsync(userId, id, dto);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _postService.DeleteAsync(userId, id);
                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
    }
}