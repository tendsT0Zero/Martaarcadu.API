using Martaarcadu.Application.DTOs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martaarcadu.Application.Interfaces
{
    public interface IChatNotificationService
    {
        Task NotifyNewMessageAsync(string conversationId, MessageResponseDto message);
    }
}
