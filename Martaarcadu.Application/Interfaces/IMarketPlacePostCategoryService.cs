using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.MarketPlacePostCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface IMarketPlacePostCategoryService
    {
        Task<APIResponseDto> GetAllAsync();
        Task<APIResponseDto> GetByIdAsync(Guid id);
        Task<APIResponseDto> CreateAsync(CreateMarketPlacePostCategory dto);
        Task<APIResponseDto> UpdateAsync(Guid id, UpdateMarketPlacePostCategory dto);
        Task<APIResponseDto> DeleteAsync(Guid id);
    }
}
