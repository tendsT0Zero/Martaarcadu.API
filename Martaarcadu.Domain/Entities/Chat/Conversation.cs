using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Domain.Entities.Chat
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;

        // Relation to the specific Post being discussed
        public Guid PostId { get; set; }
        [ForeignKey("PostId")]
        public MarketPlacePost.MarketPlacePost Post { get; set; }

        // The owner of the post
        public Guid PostOwnerId { get; set; }
        [ForeignKey("PostOwnerId")]
        public ApplicationUser.ApplicationUser PostOwner { get; set; }

        // The user interested in the post (The Buyer/Inquirer)
        public Guid ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public ApplicationUser.ApplicationUser Participant { get; set; }

        public ICollection<ChatMessage> Messages { get; set; }
    }
}
