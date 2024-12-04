using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebUI.Core.DbContext;
using WebUI.Core.Dtos.General;
using WebUI.Core.Dtos.Message;
using WebUI.Core.Entities;
using WebUI.Core.Interfaces;

namespace WebUI.Core.Services
{
    public class MessageService : IMessageService
    {
        #region Constructor & DI
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageService(ApplicationDbContext context, ILogService logService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logService = logService;
            _userManager = userManager;
        }
        #endregion

        #region CreateNewMessageAsync
        public async Task<GeneralServiceResponseDTO> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDTO createMessageDto)
        {
            if (User.Identity.Name == createMessageDto.ReceiverUserName)
                return new GeneralServiceResponseDTO()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Sender and Receiver can not be same",
                };

            var isReceiverUserNameValid = _userManager.Users.Any(q => q.UserName == createMessageDto.ReceiverUserName);
            if (!isReceiverUserNameValid)
                return new GeneralServiceResponseDTO()
                {
                    IsSucceed = false,
                    StatusCode = 400,
                    Message = "Receiver UserName is not valid",
                };

            var newMessage = new Message()
            {
                SenderUserName = User.Identity.Name,
                ReceiverUserName = createMessageDto.ReceiverUserName,
                Text = createMessageDto.Text
            };
            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            await _logService.SaveNewLog(User.Identity.Name, "Send Message");

            return new GeneralServiceResponseDTO()
            {
                IsSucceed = true,
                StatusCode = 201,
                Message = "Message saved successfully",
            };
        }
        #endregion

        #region GetMessagesAsync
        public async Task<IEnumerable<GetMessageDTO>> GetMessagesAsync()
        {
            var messages = await _context.Messages
                .Select(q => new GetMessageDTO()
                {
                    Id = q.Id,
                    SenderUserName = q.SenderUserName,
                    ReceiverUserName = q.ReceiverUserName,
                    Text = q.Text,
                    CreatedAt = q.CreatedAt
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();

            return messages;
        }
        #endregion

        #region GetMyMessagesAsync
        public async Task<IEnumerable<GetMessageDTO>> GetMyMessagesAsync(ClaimsPrincipal User)
        {
            var loggedInUser = User.Identity.Name;

            var messages = await _context.Messages
                .Where(q => q.SenderUserName == loggedInUser || q.ReceiverUserName == loggedInUser)
                 .Select(q => new GetMessageDTO()
                 {
                     Id = q.Id,
                     SenderUserName = q.SenderUserName,
                     ReceiverUserName = q.ReceiverUserName,
                     Text = q.Text,
                     CreatedAt = q.CreatedAt
                 })
                 .OrderByDescending(q => q.CreatedAt)
                 .ToListAsync();

            return messages;
        }
        #endregion
    }
}
