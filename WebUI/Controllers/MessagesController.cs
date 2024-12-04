using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Core.Constants;
using WebUI.Core.Dtos.Message;
using WebUI.Core.Interfaces;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Route -> Create a new message to send to another user
        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewMessage([FromBody] CreateMessageDTO createMessageDto)
        {
            var result = await _messageService.CreateNewMessageAsync(User, createMessageDto);
            if (result.IsSucceed)
                return Ok(result.Message);

            return StatusCode(result.StatusCode, result.Message);
        }

        // Route -> Get All Messages for current user, Either as Sender or as Receiver
        [HttpGet]
        [Route("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetMessageDTO>>> GetMyMessages()
        {
            var messages = await _messageService.GetMyMessagesAsync(User);
            return Ok(messages);
        }

        // Route -> Get all messages With Owner access and Admin access
        [HttpGet]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<GetMessageDTO>>> GetMessages()
        {
            var messages = await _messageService.GetMessagesAsync();
            return Ok(messages);
        }
    }
}
