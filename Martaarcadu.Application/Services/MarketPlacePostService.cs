using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.MarketPlacePost;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Domain.Entities.Location;
using Martaarcadu.Domain.Entities.MarketPlacePost;
using Martaarcadu.Domain.Entities.PostImage;
using Martaarcadu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Martaarcadu.Application.DTOs.MarketPlacePost.MarketPlacePostResponse;

namespace Martaarcadu.Application.Services
{
    public class MarketPlacePostService:IMarketPlacePostService
    {
        private readonly AppDbContext _context;
        private readonly IPhotoUploadService _photoUploadService; 

        public MarketPlacePostService(AppDbContext context, IPhotoUploadService photoUploadService)
        {
            _context = context;
            _photoUploadService = photoUploadService;
        }

        public async Task<APIResponseDto> GetAllAsync()
        {
            try
            {
                var posts = await _context.MarketPlacePosts
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.Location)
                    .Include(p => p.Images)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                var dtos = posts.Select(MapToDto).ToList();

                return new APIResponseDto
                {
                    ResponseObject = dtos,
                    IsSuccess = true,
                    Message="success"
                   
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = false,
                    Message = ex.Message

                };
            }
        }

        public async Task<APIResponseDto> GetByIdAsync(Guid id)
        {
            try
            {
                var post = await _context.MarketPlacePosts
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.Location)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (post == null)
                    return new APIResponseDto
                    {
                        ResponseObject = null,
                        IsSuccess = true,
                        Message = "success"

                    };

                return new APIResponseDto
                {
                    ResponseObject = MapToDto(post),
                    IsSuccess = true,
                    Message = "success"

                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = false,
                    Message = ex.Message

                };
            }
        }

        public async Task<APIResponseDto> GetMyPostsAsync(Guid userId)
        {
            try
            {
                var posts = await _context.MarketPlacePosts
                    .Include(p => p.Category)
                    .Include(p => p.Location)
                    .Include(p => p.Images)
                    .Where(p => p.UserId == userId)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();

                
                var dtos = posts.Select(p => MapToDto(p, userId)).ToList();

                return new APIResponseDto
                {
                    ResponseObject = dtos,
                    IsSuccess = true,
                    Message = "success"

                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = false,
                    Message = ex.Message

                };
            }
        }

        public async Task<APIResponseDto> CreateAsync(Guid userId, CreateMarketPlacePostDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create Post Entity
                var post = new MarketPlacePost
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CategoryId = dto.CategoryId,
                    Title = dto.Title,
                    Description = dto.Description,
                    Budget = dto.Budget,
                    ResponseTime = dto.ResponseTime,
                    Status = "Open",
                    CreatedAt = DateTime.UtcNow
                };

                // Add Location
                post.Location = new Location
                {
                    Id = Guid.NewGuid(),
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    TextAddress = dto.TextAddress
                };

                // Handle Images
                if (dto.Images != null && dto.Images.Any())
                {
                    post.Images = new List<PostImage>();
                    foreach (var imageFile in dto.Images)
                    {
                        
                        var imageUrl = await _photoUploadService.UploadPhotoAsync(imageFile);
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            post.Images.Add(new PostImage
                            {
                                Id = Guid.NewGuid(),
                                ImageUrl = imageUrl,
                                IsPrimary = post.Images.Count == 0 
                            });
                        }
                    }
                }

                _context.MarketPlacePosts.Add(post);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = true,
                    Message = "success"

                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = false,
                    Message = ex.Message

                };
            }
        }

        public async Task<APIResponseDto> UpdateAsync(Guid userId, Guid postId, UpdateMarketPlacePostDto dto)
        {
            try
            {
                var post = await _context.MarketPlacePosts.FindAsync(postId);

                if (post == null)
                    return new APIResponseDto
                    {
                        ResponseObject = null,
                        IsSuccess = false,
                        Message = "Post not found"

                    };

                // Security Check: Ensure the user owns the post
                //if (post.UserId != userId)
                //    return new APIResponseDto(false, "Unauthorized: You do not own this post.");

                post.Title = dto.Title;
                post.Description = dto.Description;
                post.Budget = dto.Budget;
                post.Status = dto.Status;

                _context.MarketPlacePosts.Update(post);
                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = true,
                    Message = "success"

                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = false,
                    Message = ex.Message

                };
            }
        }

        public async Task<APIResponseDto> DeleteAsync(Guid userId, Guid postId)
        {
            try
            {
                var post = await _context.MarketPlacePosts.FindAsync(postId);

                if (post == null)
                    return new APIResponseDto
                    {
                        ResponseObject = null,
                        IsSuccess = false,
                        Message = "Post not found"

                    };

                // Security Check: Ensure the user owns the post
                //if (post.UserId != userId)
                //    return new APIResponseDto(false, "Unauthorized: You do not own this post.");

                _context.MarketPlacePosts.Remove(post);
                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = true,
                    Message = "success"

                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = false,
                    Message = ex.Message

                };
            }
        }

        // Helper Method
        private MarketPlacePostResponseDto MapToDto(MarketPlacePost post)
        {
            return MapToDto(post, post.UserId);
        }

        private MarketPlacePostResponseDto MapToDto(MarketPlacePost post, Guid userId)
        {
            return new MarketPlacePostResponseDto
            {
                Id = post.Id,
                UserId = userId,
                UserName = post.User?.FullName ?? "Unknown",
                UserPhotoUrl = post.User?.ProfilePhotoUrl,
                CategoryId = post.CategoryId,
                CategoryName = post.Category?.Name.ToString() ?? "Unknown",
                Title = post.Title,
                Description = post.Description,
                Budget = post.Budget,
                Status = post.Status,
                CreatedAt = post.CreatedAt,
                Location = post.Location != null ? new LocationDto
                {
                    Latitude = post.Location.Latitude,
                    Longitude = post.Location.Longitude,
                    TextAddress = post.Location.TextAddress
                } : null,
                Images = post.Images?.Select(i => new PostImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    IsPrimary = i.IsPrimary
                }).ToList()
            };
        }
    }

}
