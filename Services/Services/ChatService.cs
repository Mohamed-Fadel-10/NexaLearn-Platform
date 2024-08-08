using Infrastructure.Data;
using Infrastructure.ViewModels;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ChatService:IChatService
    {
        private readonly QuizContext _context;
        public ChatService(QuizContext context) { _context = context; }

        public async Task<List<SectionMessagesViewModel>> GetSectionChat(int sectionId)
        {
            var sectionChat = await _context.Messages
                .Join(_context.Sections,
                      m => m.SectionId,
                      s => s.Id,
                      (m, s) => new { Message = m, Section = s })
                .Join(_context.Users,
                      ms => ms.Message.UserId,
                      u => u.Id,
                      (ms, u) => new { ms.Message, ms.Section, User = u })
                .Where(msu => msu.Message.SectionId == sectionId)
                .OrderBy(m=>m.Message.CreatedAt)
                .Select(msu => new SectionMessagesViewModel
                {
                    UserName = msu.User.UserName,
                    CreatedAt = msu.Message.CreatedAt,
                    Text = msu.Message.Text,
                    UserId = msu.User.Id ,
                    PhotoPath=msu.User.Photo != null ? $"/userImages/{msu.User.Photo}" : null,
                }).ToListAsync();

            return sectionChat.Any() ? sectionChat : new List<SectionMessagesViewModel>();
        }

    }
}
