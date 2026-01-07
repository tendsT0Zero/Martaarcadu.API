using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Domain.Entities.PostImage
{
    public class PostImage
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }
        [ForeignKey("PostId")]
        public MarketPlacePost.MarketPlacePost Post { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsPrimary { get; set; }
    }
}
