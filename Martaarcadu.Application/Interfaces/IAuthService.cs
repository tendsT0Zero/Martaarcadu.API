using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface IAuthService
    {
        Task<APIResponseDto> RegisterAsync(UserRegistrationDto model);
        Task<APIResponseDto> LoginAsync(LoginDto model);
        Task<APIResponseDto> VerifyEmailAsync(VerifyEmailDto model);
        Task<APIResponseDto> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<APIResponseDto> ResetPasswordAsync(ResetPasswordDto model);
    }
}
