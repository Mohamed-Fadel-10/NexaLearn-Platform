using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Infrastructure.Data;
using Infrastructure.Hubs;
using Services.Interfaces;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;

namespace YourNamespace.Controllers
{
    public class ChatRoomController : Controller
    {
        private readonly ISectionService _sectionService;
        private readonly QuizContext _context;
        private readonly IHubContext<ChatHub> _hub;
        private readonly IChatService _chatService;

        public ChatRoomController(ISectionService sectionService, QuizContext context,
            IHubContext<ChatHub> hub, IChatService _chatService)
        {
            _sectionService = sectionService;
            _context = context;
            _hub = hub;
            this._chatService = _chatService;
        }
        [Authorize]
        public async Task< IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.userid = userId;
            var response = await _sectionService.StudentSections(userId);
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(string message, int sectionId, string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var newMessage = new Message
            {
                CreatedAt = DateTime.Now,
                SectionId = sectionId,
                Text = message,
                UserId = userId,
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            var photoPath = user.Photo != null ? $"/userImages/{user.Photo}" : null;

            await _hub.Clients.Group($"Section-{sectionId}").SendAsync(
                "ReceiveMessage",
                message,
                user.UserName,
                userId,
                photoPath,
                newMessage.CreatedAt
            );

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> SectionChat(int sectionId)
        {
            var response = await _chatService.GetSectionChat(sectionId);
            return response.Any() ? Ok(response) : BadRequest();
        }


    }
}
