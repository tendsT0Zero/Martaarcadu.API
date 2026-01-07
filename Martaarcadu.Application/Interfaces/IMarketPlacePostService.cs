using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.MarketPlacePost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface IMarketPlacePostService
    {
        Task<APIResponseDto> GetAllAsync(); 
        Task<APIResponseDto> GetByIdAsync(Guid id);
        Task<APIResponseDto> CreateAsync(Guid userId, CreateMarketPlacePostDto dto);
        Task<APIResponseDto> UpdateAsync(Guid userId, Guid postId, UpdateMarketPlacePostDto dto);
        Task<APIResponseDto> DeleteAsync(Guid userId, Guid postId);
        Task<APIResponseDto> GetMyPostsAsync(Guid userId);
    }
}
