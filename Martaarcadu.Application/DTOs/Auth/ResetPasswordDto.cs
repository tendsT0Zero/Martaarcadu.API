using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.Auth
{
    public class ResetPasswordDto
    {
        [Required][EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Otp { get; set; } = string.Empty; // The OTP/Token
        [Required][MinLength(6)] public string NewPassword { get; set; } = string.Empty;
        [Required][MinLength(6)] public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
