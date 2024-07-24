using System.ComponentModel.DataAnnotations;

namespace IziWork.Business.Args
{
    public class UserLoginRequest
    {
        [Required]
        public string LoginName { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
