using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinSection(int sectionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Section-{sectionId}");
        }

        public async Task LeaveSection(int sectionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Section-{sectionId}");
        }
    }
}
