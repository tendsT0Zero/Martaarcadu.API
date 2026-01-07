using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Martaarcadu.Domain.Enums.Enums;

namespace Martaarcadu.Domain.Entities.MarketPlacePostCategory
{
    public class MarketPlacePostCategory
    {
        public Guid Id { get; set; }
        [Required]
        public PostType Name { get; set; }
    }
}
