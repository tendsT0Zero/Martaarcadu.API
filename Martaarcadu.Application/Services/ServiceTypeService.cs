using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.ServiceType;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Domain.Entities.Service;
using Martaarcadu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Martaarcadu.Application.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly AppDbContext _context;

        public ServiceTypeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponseDto> GetAllAsync()
        {
            try
            {
                var services = await _context.ServiceTypes
                    .Select(s => new ServiceTypeResponseDto { ServiceId = s.Id, ServiceName = s.Name })
                    .ToListAsync();

                return new APIResponseDto
                {
                    ResponseObject = services,
                    IsSuccess = true,
                    Message = "Success."
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = $"Error retrieving services: {ex.Message}"
                };
            }
        }

        public async Task<APIResponseDto> GetByIdAsync(Guid id)
        {
            try
            {
                var service = await _context.ServiceTypes.FindAsync(id);
                if (service == null)
                {
                    return new APIResponseDto
                    {
                        IsSuccess = false,
                        Message = "Not found with the provided ID."
                    };
                }

                return new APIResponseDto
                {
                    ResponseObject = new ServiceTypeResponseDto
                    {
                        ServiceId = service.Id,
                        ServiceName = service.Name,
                    },
                    IsSuccess = true,
                    Message = "Success."
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = $"Error retrieving service: {ex.Message}"
                };
            }
        }

        public async Task<APIResponseDto> CreateAsync(CreateServiceTypeDto dto)
        {
            try
            {
                var service = new ServiceType { Name = dto.Name };
                _context.ServiceTypes.Add(service);
                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Success",
                    ResponseObject = service.Id
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = $"Error creating service: {ex.Message}"
                };
            }
        }

        public async Task<APIResponseDto> UpdateAsync(Guid id, UpdateServiceTypeDto dto)
        {
            try
            {
                var service = await _context.ServiceTypes.FindAsync(id);
                if (service == null)
                {
                    return new APIResponseDto
                    {
                        IsSuccess = false,
                        Message = "Not Found with the given ID"
                    };
                }

                service.Name = dto.Name;
                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = $"Error updating service: {ex.Message}"
                };
            }
        }

        public async Task<APIResponseDto> DeleteAsync(Guid id)
        {
            try
            {
                var service = await _context.ServiceTypes.FindAsync(id);
                if (service == null)
                {
                    return new APIResponseDto
                    {
                        IsSuccess = false,
                        Message = "Not found Service with the Given Id."
                    };
                }

                _context.ServiceTypes.Remove(service);
                await _context.SaveChangesAsync();

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Success"
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto
                {
                    IsSuccess = false,
                    Message = $"Error deleting service: {ex.Message}"
                };
            }
        }
    }
}