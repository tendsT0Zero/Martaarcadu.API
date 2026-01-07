using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.Chat
{
    public class SendMessageDto
    {
        [Required]
        public Guid PostId { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
        public string Content { get; set; } = string.Empty;

    }
}
