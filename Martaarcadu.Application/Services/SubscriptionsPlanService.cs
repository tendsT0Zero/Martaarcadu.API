using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.SubscriptionPlan;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Domain.Entities.SubscriptionPlan;
using Martaarcadu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static Martaarcadu.Domain.Enums.Enums;

namespace Martaarcadu.Application.Services
{
    public class SubscriptionsPlanService : ISubscriptionsPlanService
    {
        private readonly AppDbContext _context;

        public SubscriptionsPlanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponseDto> GetAllAsync()
        {
            try
            {
                var plans = await _context.SubscriptionPlans
                    .Include(p => p.Benefits) 
                    .ToListAsync();

                var response = plans.Select(p=> new SubscriptionsPlanDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Duration = p.Duration,
                    Price = p.Price,
                    Benefits=p.Benefits?.Select(b=>b.Description).ToList(),
                }).ToList();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Success",
                    ResponseObject = response
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
                var plan = await _context.SubscriptionPlans
                    .Include(p => p.Benefits)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (plan == null)
                    return new APIResponseDto { IsSuccess = false, Message = "Plan not found." };

                var response = new SubscriptionsPlanDto
                {
                    Id = id,
                    Name = plan.Name,
                    Price = plan.Price,
                    Duration = plan.Duration,
                    Benefits = plan.Benefits?.Select(b => b.Description).ToList()
                };

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Success",
                    ResponseObject = response
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<APIResponseDto> CreateAsync(CreatePlanDto dto)
        {
            try
            {
                //Validate that the Duration is a valid Enum value
                if (!Enum.IsDefined(typeof(DurationType), dto.Duration))
                {
                    return new APIResponseDto
                    {
                        IsSuccess = false,
                        Message = "Invalid duration type. Accepted values are: Monthly, Yearly, Free."
                    };
                }

                //Check if the Name is unique
                bool nameExists = await _context.SubscriptionPlans
                    .AnyAsync(p => p.Name.ToLower() == dto.Name.ToLower()); // Case-insensitive check recommended

                if (nameExists)
                {
                    return new APIResponseDto
                    {
                        IsSuccess = false,
                        Message = "A subscription plan with this name already exists."
                    };
                }
                var plan = new SubscriptionsPlan 
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Duration = dto.Duration,
                    Benefits = new List<SubscriptionPlanBenefit>()
                };

         
                if (dto.BenefitIds != null && dto.BenefitIds.Any())
                {
                    var existingBenefits = await _context.SubscriptionPlanBenefits
                        .Where(b => dto.BenefitIds.Contains(b.Id))
                        .ToListAsync();


                    foreach (var benefit in existingBenefits)
                    {
                        plan.Benefits.Add(benefit);
                    }
                }

                _context.SubscriptionPlans.Add(plan);
                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Plan created successfully",
                    ResponseObject = plan.Id
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<APIResponseDto> UpdateAsync(Guid id, UpdatePlanDto dto)
        {
            try
            {
                // Validate that the Duration is a valid Enum value
                if (!Enum.IsDefined(typeof(DurationType), dto.Duration))
                {
                    return new APIResponseDto
                    {
                        IsSuccess = false,
                        Message = "Invalid duration type. Accepted values are: Monthly, Yearly, Free."
                    };
                }

                var plan = await _context.SubscriptionPlans
                    .Include(p => p.Benefits)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (plan == null)
                    return new APIResponseDto { IsSuccess = false, Message = "Plan not found." };

                // Check for unique name
                bool nameExists = await _context.SubscriptionPlans
                    .AnyAsync(p => p.Name.ToLower() == dto.Name.ToLower() && p.Id != id);

                if (nameExists)
                {
                    return new APIResponseDto
                    {
                        IsSuccess = false,
                        Message = "A subscription plan with this name already exists."
                    };
                }


                plan.Name = dto.Name;
                plan.Price = dto.Price;
                plan.Duration = dto.Duration;

            
                if (dto.BenefitIds != null)
                {
                    var targetBenefits = await _context.SubscriptionPlanBenefits
                        .Where(b => dto.BenefitIds.Contains(b.Id))
                        .ToListAsync();
                    plan.Benefits = targetBenefits;
                }

                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Plan updated successfully"
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
                var plan = await _context.SubscriptionPlans.FindAsync(id);
                if (plan == null)
                    return new APIResponseDto { IsSuccess = false, Message = "Plan not found." };

                _context.SubscriptionPlans.Remove(plan);
                await _context.SaveChangesAsync();

                return new APIResponseDto { IsSuccess = true, Message = "Plan deleted successfully" };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}