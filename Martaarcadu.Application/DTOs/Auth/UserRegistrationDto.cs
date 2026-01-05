using Martaarcadu.Domain.Entities.SubscriptionPlan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.Auth
{
    public class UserRegistrationDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ProfilePhotoUrl { get; set; }

        [MaxLength(1000)]
        public string? AboutMe { get; set; }

        public bool IsAgreedTermPolicy { get; set; }

        [MaxLength(500)]
        public string? TermPolicyUrl { get; set; }
    }
}
