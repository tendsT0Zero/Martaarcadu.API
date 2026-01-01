using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.SubscriptionPlanBenefitsDtos;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Domain.Entities.SubscriptionPlan;
using Martaarcadu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Martaarcadu.Application.Services
{
    public class SubscriptionsPlanBenefitService : ISubscriptionsPlanBenefitService
    {
        private readonly AppDbContext _context;

        public SubscriptionsPlanBenefitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponseDto> GetAllByPlanIdAsync(Guid planId)
        {
            try
            {
                var benefits = await _context.SubscriptionPlanBenefits
                    .Where(b => b.SubscriptionPlanId == planId)
                    .Select(b => new SubscriptionsPlanBenefitsDto
                    {
                        Id = b.Id,
                        Descripcion = b.Description 
                    })
                    .ToListAsync();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Success",
                    ResponseObject = benefits
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<APIResponseDto> GetByIdAsync(Guid id)
        {
            try
            {
                var benefit = await _context.SubscriptionPlanBenefits.FindAsync(id);
                if (benefit == null)
                    return new APIResponseDto { IsSuccess = false, Message = "Benefit not found." };

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Success",
                    ResponseObject = new SubscriptionsPlanBenefitsDto
                    {
                        Id = benefit.Id,
                        Descripcion = benefit.Description
                    }
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<APIResponseDto> CreateAsync(Guid planId, CreatePlanBenefit dto)
        {
            try
            {
                // Verify the Plan exists first
                var planExists = await _context.SubscriptionPlans.AnyAsync(p => p.Id == planId);
                if (!planExists)
                    return new APIResponseDto { IsSuccess = false, Message = "Subscription Plan not found." };

                // Create Entity
                var benefit = new SubscriptionPlanBenefit
                {
                    SubscriptionPlanId = planId,
                    Description = dto.Description
                };

                _context.SubscriptionPlanBenefits.Add(benefit);
                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Benefit created successfully",
                    ResponseObject = benefit.Id
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<APIResponseDto> UpdateAsync(Guid id, UpdatePlanBenefit dto)
        {
            try
            {
                var benefit = await _context.SubscriptionPlanBenefits.FindAsync(id);
                if (benefit == null)
                    return new APIResponseDto { IsSuccess = false, Message = "Benefit not found." };

                benefit.Description = dto.Description;
                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Benefit updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<APIResponseDto> DeleteAsync(Guid id)
        {
            try
            {
                var benefit = await _context.SubscriptionPlanBenefits.FindAsync(id);
                if (benefit == null)
                    return new APIResponseDto { IsSuccess = false, Message = "Benefit not found." };

                _context.SubscriptionPlanBenefits.Remove(benefit);
                await _context.SaveChangesAsync();

                return new APIResponseDto { IsSuccess = true, Message = "Benefit deleted successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}