using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.SubscriptionPlanBenefitsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface ISubscriptionsPlanBenefitService
    {
        Task<APIResponseDto> GetAllByPlanIdAsync(Guid planId); 
        Task<APIResponseDto> GetByIdAsync(Guid id);
        Task<APIResponseDto> CreateAsync(Guid planId, CreatePlanBenefit dto);
        Task<APIResponseDto> UpdateAsync(Guid id, UpdatePlanBenefit dto);
        Task<APIResponseDto> DeleteAsync(Guid id);
    }
}
