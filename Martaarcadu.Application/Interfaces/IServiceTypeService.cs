using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.ServiceType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface IServiceTypeService
    {
        Task<APIResponseDto> GetAllAsync();
        Task<APIResponseDto> GetByIdAsync(Guid id);
        Task<APIResponseDto> CreateAsync(CreateServiceTypeDto dto);
        Task<APIResponseDto> UpdateAsync(Guid id, UpdateServiceTypeDto dto);
        Task<APIResponseDto> DeleteAsync(Guid id);
    }
}
