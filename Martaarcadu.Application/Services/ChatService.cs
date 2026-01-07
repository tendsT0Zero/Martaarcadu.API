using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.Chat;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Domain.Entities.Chat;
using Martaarcadu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Martaarcadu.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _context;
        private readonly IChatNotificationService _notifier;

        public ChatService(AppDbContext context, IChatNotificationService notifier)
        {
            _context = context;
            _notifier = notifier;
        }

        public async Task<APIResponseDto> SendMessageAsync(Guid senderId, SendMessageDto dto)
        {
            try
            {
                //Find the Post to identify the Seller
                var post = await _context.MarketPlacePosts
                    .Include(p => p.User) 
                    .FirstOrDefaultAsync(p => p.Id == dto.PostId);

                if (post == null)
                    return new APIResponseDto { IsSuccess = false, Message = "Post not found" };

                // Identify Participants
                Guid postOwnerId = post.UserId;
                Guid buyerId = senderId;

                // If the sender is the owner, we can't start a chat with ourselves (logic can be adjusted if needed)
                if (postOwnerId == senderId)
                {
                    return new APIResponseDto
                    {
                        IsSuccess = false,
                        Message="Self conversation not allowed."
                    };
                }

                
                var conversation = await _context.Conversations
                    .FirstOrDefaultAsync(c => c.PostId == post.Id &&
                                            (c.ParticipantId == senderId || c.PostOwnerId == senderId));

                if (conversation == null)
                {
                    conversation = new Conversation
                    {
                        Id = Guid.NewGuid(),
                        PostId = post.Id,
                        PostOwnerId = postOwnerId,
                        ParticipantId = senderId,
                        CreatedAt = DateTime.UtcNow,
                        LastMessageAt = DateTime.UtcNow
                    };
                    _context.Conversations.Add(conversation);
                }

                // Create Message
                var chatMessage = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    ConversationId = conversation.Id,
                    SenderId = senderId,
                    Content = dto.Content,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                _context.ChatMessages.Add(chatMessage);

                // Update Conversation Metadata
                conversation.LastMessageAt = chatMessage.SentAt;

                await _context.SaveChangesAsync();

                // Response & Notify SignalR
                var responseDto = new MessageResponseDto
                {
                    Id = chatMessage.Id,
                    ConversationId = conversation.Id,
                    SenderId = senderId,
                    Content = chatMessage.Content,
                    SentAt = chatMessage.SentAt,
                    IsRead = false
                };

                // REAL-TIME TRIGGER
                await _notifier.NotifyNewMessageAsync(conversation.Id.ToString(), responseDto);

                return new APIResponseDto
                {
                    IsSuccess = true,
                    Message = "Message sent",
                    ResponseObject = responseDto
                };
            }
            catch (Exception ex)
            {
                return new APIResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<APIResponseDto> GetMyConversationsAsync(Guid userId)
        {
            var conversations = await _context.Conversations
                .Include(c => c.Post)
                .Include(c => c.Participant)
                .Include(c => c.PostOwner)
                .Where(c => c.ParticipantId == userId || c.PostOwnerId == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();

            var dtos = conversations.Select(c => new ConversationResponseDto
            {
                Id = c.Id,
                PostId = c.PostId,
                PostTitle = c.Post?.Title ?? "Unknown",
                PostImageUrl = null, 
                OtherParticipantId = c.ParticipantId == userId ? c.PostOwnerId : c.ParticipantId,
                OtherParticipantName = c.ParticipantId == userId ? c.PostOwner?.FullName : c.Participant?.FullName,

                LastMessageAt = c.LastMessageAt
            }).ToList();

            return new APIResponseDto { IsSuccess = true, ResponseObject = dtos, Message = "Success" };
        }

        public async Task<APIResponseDto> GetMessagesAsync(Guid userId, Guid conversationId)
        {
            var messages = await _context.ChatMessages
               .Where(m => m.ConversationId == conversationId)
               .OrderBy(m => m.SentAt)
               .ToListAsync();

            var dtos = messages.Select(m => new MessageResponseDto
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                SenderId = m.SenderId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            }).ToList();

            return new APIResponseDto { IsSuccess = true, ResponseObject = dtos, Message = "Success" };
        }
    }
}