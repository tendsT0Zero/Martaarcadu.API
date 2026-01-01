using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.SubscriptionPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface ISubscriptionsPlanService
    {
        Task<APIResponseDto> GetAllAsync();
        Task<APIResponseDto> GetByIdAsync(Guid id);
        Task<APIResponseDto> CreateAsync(CreatePlanDto dto);
        Task<APIResponseDto> UpdateAsync(Guid id, UpdatePlanDto dto);
        Task<APIResponseDto> DeleteAsync(Guid id);
    }
}
