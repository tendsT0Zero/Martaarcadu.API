using Martaarcadu.Application.DTOs.APIResponse;
using Martaarcadu.Application.DTOs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface IChatService
    {
        Task<APIResponseDto> SendMessageAsync(Guid senderId, SendMessageDto dto);
        Task<APIResponseDto> GetMyConversationsAsync(Guid userId);
        Task<APIResponseDto> GetMessagesAsync(Guid userId, Guid conversationId);
    }
}
