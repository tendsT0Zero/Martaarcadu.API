using Martaarcadu.Domain.Entities.Service;
using Martaarcadu.Domain.Entities.SubscriptionPlan;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Domain.Entities.ApplicationUser
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; } = string.Empty;

        public Guid? SubscriptionPlanId { get; set; }

        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionsPlan? SubscriptionPlan { get; set; }


        //Professional Info
        [MaxLength(500)]
        public string? ProfilePhotoUrl { get; set; }

        [MaxLength(1000)]
        public string? AboutMe { get; set; }

        public bool IsAgreedTermPolicy { get; set; }

        [MaxLength(500)]
        public string? TermPolicyUrl { get; set; }

        public  ICollection<ServiceType>? ServiceTypes { get; set; }
    }
}
