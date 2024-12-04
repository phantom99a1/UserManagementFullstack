using System.Security.Claims;
using WebUI.Core.Dtos.Auth;
using WebUI.Core.Dtos.General;

namespace WebUI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralServiceResponseDTO> SeedRolesAsync();
        Task<GeneralServiceResponseDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<LoginServiceResponseDTO?> LoginAsync(LoginDTO loginDTO);
        Task<GeneralServiceResponseDTO> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDTO updateRoleDTO);
        Task<LoginServiceResponseDTO?> MeAsync(MeDTO meDTO);
        Task<IEnumerable<UserInfoResult>> GetUsersListAsync();
        Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName);
        Task<IEnumerable<string>> GetUsernamesListAsync();
    }
}
