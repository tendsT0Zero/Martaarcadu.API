using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Domain.Entities.Location
{
    public class Location
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }
        [ForeignKey("PostId")]
        public MarketPlacePost.MarketPlacePost Post { get; set; }

        [Column(TypeName = "decimal(18,8)")] 
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal Longitude { get; set; }

        [MaxLength(200)]
        public string TextAddress { get; set; } = string.Empty;
    }
}
