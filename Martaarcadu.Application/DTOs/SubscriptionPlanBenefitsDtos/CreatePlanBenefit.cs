using Martaarcadu.Domain.Entities.SubscriptionPlan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.SubscriptionPlanBenefitsDtos
{
    public class CreatePlanBenefit
    {

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
