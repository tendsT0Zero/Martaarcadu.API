using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Domain.Entities.MarketPlacePost
{
    public class MarketPlacePost
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser.ApplicationUser User { get; set; }

        public Guid CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public MarketPlacePostCategory.MarketPlacePostCategory Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Budget { get; set; }

        public DateTime? ResponseTime { get; set; }

        public long ResponseCount { get; set; }

        // You might want to create an Enum for this in Enums.cs
        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Open"; // Open, InProgress, Completed, Cancelled

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        // Relationships
        public ICollection<PostImage.PostImage>? Images { get; set; }
        public Location.Location? Location { get; set; }
    }
}
