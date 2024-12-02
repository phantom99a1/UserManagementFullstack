using System.ComponentModel.DataAnnotations;

namespace WebUI.Core.Dtos.Auth
{
    public class UpdateRoleDTO
    {
        [Required(ErrorMessage = " UserName is required")]
        public string UserName { get; set; }
        public RoleType NewRole { get; set; }
    }

    public enum RoleType
    {
        ADMIN,
        MANAGER,
        USER
    }
}
