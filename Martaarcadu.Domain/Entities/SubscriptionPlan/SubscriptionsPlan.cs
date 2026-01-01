using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Martaarcadu.Domain.Enums.Enums;

namespace Martaarcadu.Domain.Entities.SubscriptionPlan
{
    public class SubscriptionsPlan
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        public DurationType Duration { get; set; } = DurationType.Free;
        public  ICollection<SubscriptionPlanBenefit>? Benefits { get; set; }
    }
}
