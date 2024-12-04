using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Core.Constants;
using WebUI.Core.Dtos.Log;
using WebUI.Core.Interfaces;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<GetLogDTO>>> GetLogs()
        {
            var logs = await _logService.GetLogsAsync();
            return Ok(logs);
        }

        [HttpGet]
        [Route("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetLogDTO>>> GetMyLogs()
        {
            var logs = await _logService.GetMyLogsAsync(User);
            return Ok(logs);
        }
    }
}
