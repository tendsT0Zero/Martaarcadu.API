using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.MarketPlacePost
{
    public class MarketPlacePostResponse
    {
        public class MarketPlacePostResponseDto
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public string UserName { get; set; } = string.Empty; // Useful for UI
            public string UserPhotoUrl { get; set; } // Useful for UI
            public Guid CategoryId { get; set; }
            public string CategoryName { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Budget { get; set; }
            public string Status { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public LocationDto? Location { get; set; }
            public List<PostImageDto>? Images { get; set; }
        }

       
    }
}
