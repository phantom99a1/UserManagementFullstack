using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebUI.Core.DbContext;
using WebUI.Core.Dtos.Log;
using WebUI.Core.Entities;
using WebUI.Core.Interfaces;

namespace WebUI.Core.Services
{
    public class LogService : ILogService
    {
        #region Constructor & DI
        private readonly ApplicationDbContext _context;

        public LogService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region SaveNewLog
        public async Task SaveNewLog(string UserName, string Description)
        {
            var newLog = new Log()
            {
                UserName = UserName,
                Description = Description
            };

            await _context.Logs.AddAsync(newLog);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region GetLogsAsync
        public async Task<IEnumerable<GetLogDTO>> GetLogsAsync()
        {
            var logs = await _context.Logs
                 .Select(q => new GetLogDTO
                 {
                     CreatedAt = q.CreatedAt,
                     Description = q.Description,
                     UserName = q.UserName,
                 })
                 .OrderByDescending(q => q.CreatedAt)
                 .ToListAsync();
            return logs;
        }
        #endregion

        #region GetMyLogsAsync
        public async Task<IEnumerable<GetLogDTO>> GetMyLogsAsync(ClaimsPrincipal User)
        {
            var logs = await _context.Logs
                .Where(q => q.UserName == User.Identity.Name)
               .Select(q => new GetLogDTO
               {
                   CreatedAt = q.CreatedAt,
                   Description = q.Description,
                   UserName = q.UserName,
               })
               .OrderByDescending(q => q.CreatedAt)
               .ToListAsync();
            return logs;
        }
        #endregion
    }
}
