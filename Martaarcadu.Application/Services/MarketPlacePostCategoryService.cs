using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.MarketPlacePostCategory;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Domain.Entities.MarketPlacePostCategory;
using Martaarcadu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Martaarcadu.Domain.Enums.Enums;

namespace Martaarcadu.Application.Services
{
    public class MarketPlacePostCategoryService:IMarketPlacePostCategoryService
    {
        private readonly AppDbContext _context;

        public MarketPlacePostCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponseDto> GetAllAsync()
        {
            try
            {
                var categories = await _context.MarketPlacePostCategories.ToListAsync();

                var categoryDtos= categories.Select(c => new MarktetPlacePostCategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name.ToString() // Convert Enum back to string for response
                }).ToList();

                return new APIResponseDto
                {
                    ResponseObject = categoryDtos,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
             return new APIResponseDto
                {
                    IsSuccess=false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<APIResponseDto> GetByIdAsync(Guid id)
        {
            try
            {
                var category = await _context.MarketPlacePostCategories.FindAsync(id);
                if (category == null) return null;

                var dto= new MarktetPlacePostCategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name.ToString()
                };
                return new APIResponseDto
                {
                    ResponseObject = dto,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<APIResponseDto> CreateAsync(CreateMarketPlacePostCategory dto)
        {
            try
            {
                // Validating that the string sent matches the Enum (Service or Product)
                if (!Enum.TryParse<PostType>(dto.Name, true, out var parsedName))
                {
                    throw new ArgumentException($"Invalid category name. Allowed values: {string.Join(", ", Enum.GetNames(typeof(PostType)))}");
                }

                var category = new MarketPlacePostCategory
                {
                    Id = Guid.NewGuid(),
                    Name = parsedName
                };

                _context.MarketPlacePostCategories.Add(category);
                await _context.SaveChangesAsync();

                var result= new MarktetPlacePostCategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name.ToString()
                };
                return new APIResponseDto
                {
                    ResponseObject= result,
                    IsSuccess = true,
                    Message="Success"
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }

        }

        public async Task<APIResponseDto> UpdateAsync(Guid id, UpdateMarketPlacePostCategory dto)
        {
            try
            {
                var category = await _context.MarketPlacePostCategories.FindAsync(id);
                if (category == null) return null;

                // Validate Enum conversion
                if (!Enum.TryParse<PostType>(dto.Name, true, out var parsedName))
                {
                    throw new ArgumentException($"Invalid category name. Allowed values: {string.Join(", ", Enum.GetNames(typeof(PostType)))}");
                }

                category.Name = parsedName;

                _context.MarketPlacePostCategories.Update(category);
                await _context.SaveChangesAsync();

                var result= new MarktetPlacePostCategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name.ToString()
                };
                return new APIResponseDto
                {
                    ResponseObject = result,
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<APIResponseDto> DeleteAsync(Guid id)
        {
            try
            {
                var category = await _context.MarketPlacePostCategories.FindAsync(id);
                if (category == null) {
                    return new APIResponseDto
                    {
                        ResponseObject= null,
                        IsSuccess = false,
                        Message="Category not found with the given id."
                    };
                }

                _context.MarketPlacePostCategories.Remove(category);
                await _context.SaveChangesAsync();
                return new APIResponseDto
                {
                    ResponseObject = null,
                    IsSuccess = true,
                    Message = "Success."
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }

        }
    }
}
