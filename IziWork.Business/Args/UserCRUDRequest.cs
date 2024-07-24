using IziWork.Business.Enums;
using System.ComponentModel.DataAnnotations;

namespace IziWork.Business.Args
{
    public class UserCRUDRequest
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string LoginName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string Password { get; set; } = string.Empty;
        public GenderEnum? Gender { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool? IsActivated { get; set; }
        public AccountTypeEnum? Type { get; set; }
        public bool? IsBlocked { get; set; }
    }
}
