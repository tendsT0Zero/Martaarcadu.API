using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.MarketPlacePost
{
    public class CreateMarketPlacePostDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public decimal Budget { get; set; }
        public DateTime? ResponseTime { get; set; }

        // Location Data
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string TextAddress { get; set; } = string.Empty;

        // Images
        public List<IFormFile>? Images { get; set; }
    }
}
