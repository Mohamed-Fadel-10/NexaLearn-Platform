using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Hubs
{
    public class QuizHub:Hub
    {

        public async Task NotifyAdmin(string userId, int quizId, string submissionDate)
        {
            await Clients.All.SendAsync("ReceiveNotification", userId, quizId, submissionDate);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
