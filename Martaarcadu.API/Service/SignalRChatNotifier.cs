using Martaarcadu.Application.DTOs.Chat;
using Martaarcadu.Application.Interfaces;
using Martaarcadu.Infrastructure.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Martaarcadu.API.Service
{
    public class SignalRChatNotifier : IChatNotificationService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SignalRChatNotifier(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyNewMessageAsync(string conversationId, MessageResponseDto message)
        {
            
            await _hubContext.Clients.Group(conversationId)
                .SendAsync("ReceiveMessage", message);
        }
    }
}
