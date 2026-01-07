using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.DTOs.Chat
{
    public class ConversationResponseDto
    {
        public Guid Id { get; set; }

        // Context about the Product
        public Guid PostId { get; set; }
        public string PostTitle { get; set; } = string.Empty;
        public string? PostImageUrl { get; set; }

        // Context about the "Other Person" (Dynamic)
        // If I am Buyer, this shows Seller info. If I am Seller, this shows Buyer info.
        public Guid OtherParticipantId { get; set; }
        public string OtherParticipantName { get; set; } = string.Empty;
        public string? OtherParticipantPhotoUrl { get; set; }

        // Metadata
        public string LastMessage { get; set; } = string.Empty;
        public DateTime LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
    }

}
