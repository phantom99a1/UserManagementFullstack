using System.Security.Claims;
using WebUI.Core.Dtos.Log;

namespace WebUI.Core.Interfaces
{
    public interface ILogService
    {
        Task SaveNewLog(string UserName, string Description);
        Task<IEnumerable<GetLogDTO>> GetLogsAsync();
        Task<IEnumerable<GetLogDTO>> GetMyLogsAsync(ClaimsPrincipal User);
    }
}
