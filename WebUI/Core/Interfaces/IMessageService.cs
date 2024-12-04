using System.Security.Claims;
using WebUI.Core.Dtos.General;
using WebUI.Core.Dtos.Message;

namespace WebUI.Core.Interfaces
{
    public interface IMessageService
    {
        Task<GeneralServiceResponseDTO> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDTO createMessageDTO);
        Task<IEnumerable<GetMessageDTO>> GetMessagesAsync();
        Task<IEnumerable<GetMessageDTO>> GetMyMessagesAsync(ClaimsPrincipal User);
    }
}
