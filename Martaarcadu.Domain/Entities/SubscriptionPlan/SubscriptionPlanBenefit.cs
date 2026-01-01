using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Domain.Entities.SubscriptionPlan
{
    public class SubscriptionPlanBenefit
    {
        public Guid Id { get; set; }

        public Guid SubscriptionPlanId { get; set; }
        [ForeignKey("SubscriptionPlanId")]
        public  SubscriptionsPlan SubscriptionPlan { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
