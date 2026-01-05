using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.Auth;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Domain.Entities.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web; 

namespace Martaarcadu.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IConfiguration configuration,
                           IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        // REGISTER
        public async Task<APIResponseDto> RegisterAsync(UserRegistrationDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return new APIResponseDto { IsSuccess = false, Message = "User already exists." };

            // Check password match manually
            if (model.Password != model.ConfirmPassword)
            {
                return new APIResponseDto { IsSuccess = false, Message = "Password Should Match with ConfirmPassword." };
            }

            // 1. Generate OTP first
            string otp = RandomNumberGenerator.GetInt32(1000, 10000).ToString();

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FullName = model.FullName,
                EmailConfirmed = false,
                // 2. Set OTP fields here so they get saved immediately
                OtpCode = otp,
                OtpExpiration = DateTime.UtcNow.AddMinutes(5)
            };

            // 3. Create the user (this saves the User + OTP fields in one transaction)
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return new APIResponseDto { IsSuccess = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };

            // 4. Send Email
            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"Your confirmation token is: {otp}");

            return new APIResponseDto { IsSuccess = true, Message = "User registered. Please check email for verification code." };
        }

        // VERIFY EMAIL
        public async Task<APIResponseDto> VerifyEmailAsync(VerifyEmailDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return new APIResponseDto { IsSuccess = false, Message = "User not found." };

            // 1. Check if OTP matches
            if (user.OtpCode != model.Otp)
            {
                return new APIResponseDto { IsSuccess = false, Message = "Invalid OTP." };
            }

            // 2. Check if OTP is expired
            if (user.OtpExpiration < DateTime.UtcNow)
            {
                return new APIResponseDto { IsSuccess = false, Message = "OTP has expired." };
            }

            // 3. Success! Mark email as confirmed
            user.EmailConfirmed = true;

            // 4. Clear the OTP so it can't be used again
            user.OtpCode = null;
            user.OtpExpiration = null;

            // 5. Update the database
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new APIResponseDto { IsSuccess = false, Message = "Error updating user status." };

            return new APIResponseDto { IsSuccess = true, Message = "Email verified successfully." };
        }

        // LOGIN
        public async Task<APIResponseDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return new APIResponseDto { IsSuccess = false, Message = "Invalid email or password." };

            // Check if email is verified
            if (!user.EmailConfirmed)
                return new APIResponseDto { IsSuccess = false, Message = "Please verify your email address first." };

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return new APIResponseDto { IsSuccess = false, Message = "Invalid email or password." };

            // Generate JWT
            var token = await GenerateJwtToken(user);

            return new APIResponseDto
            {
                IsSuccess = true,
                Message = "Login successful",
                ResponseObject = new AuthResponseDto
                {
                    UserId = user.Id.ToString(),
                    Email = user.Email,
                    Token = token
                }
            };
        }

        // FORGOT PASSWORD (OTP)
        //public async Task<APIResponseDto> ForgotPasswordAsync(ForgotPasswordDto model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //        return new APIResponseDto { IsSuccess = false, Message = "If the email exists, an OTP has been sent." };

        //    //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    //generate 4 digit otp
            

        //    await _emailService.SendEmailAsync(user.Email, "Reset Password OTP", $"Your OTP is: {token}");

        //    return new APIResponseDto { IsSuccess = true, Message = "OTP sent to your email." };
        //}



    public async Task<APIResponseDto> ForgotPasswordAsync(ForgotPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        // Security Best Practice: Don't reveal if the email exists or not to prevent enumeration attacks.
        // Return the same success message even if user is null.
        if (user == null)
            return new APIResponseDto { IsSuccess = true, Message = "If the email exists, an OTP has been sent." };

        //Generate 4-digit OTP securely
        string otp = RandomNumberGenerator.GetInt32(1000, 10000).ToString();
        user.OtpCode = otp;
        user.OtpExpiration = DateTime.UtcNow.AddMinutes(5); 

        // 3. Update the user in the database
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return new APIResponseDto { IsSuccess = false, Message = "An error occurred while generating the OTP." };
        }

        // 4. Send Email
        await _emailService.SendEmailAsync(user.Email, "Reset Password OTP", $"Your OTP is: {otp}");

        return new APIResponseDto { IsSuccess = true, Message = "OTP sent to your email." };
    }

        // RESET PASSWORD
        public async Task<APIResponseDto> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new APIResponseDto { IsSuccess = false, Message = "User not found." };

            // --- STEP 1: Verify your Custom OTP ---
            if (user.OtpCode != model.Otp) // model.Token contains the 4-digit code
            {
                return new APIResponseDto { IsSuccess = false, Message = "Invalid OTP." };
            }

            if (user.OtpExpiration < DateTime.UtcNow)
            {
                return new APIResponseDto { IsSuccess = false, Message = "OTP has expired." };
            }

            // --- STEP 2: Password Confirmation Check ---
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return new APIResponseDto { IsSuccess = false, Message = "Passwords do not match." };
            }

            // --- STEP 3: The "Bridge" ---
            // Since we trust the user now (OTP matched), we generate the *real* token required by Identity.
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // --- STEP 4: Perform the Reset ---
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded)
            {
                return new APIResponseDto { IsSuccess = false, Message = string.Join(", ", result.Errors.Select(e => e.Description)) };
            }

            // --- STEP 5: Cleanup ---
            // Invalidate the OTP so it can't be used again
            user.OtpCode = null;
            user.OtpExpiration = null;
            await _userManager.UpdateAsync(user);

            return new APIResponseDto { IsSuccess = true, Message = "Password has been reset successfully." };
        }

        // JWT GENERATION 
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JWTSetting");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add Roles
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"]!)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}