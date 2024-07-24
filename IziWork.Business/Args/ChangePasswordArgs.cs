using System.ComponentModel.DataAnnotations;

namespace IziWork.Business.Args
{
    public class ChangePasswordArgs
    {
        [Required]
        public string LoginName { get; set; } = string.Empty;
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
        [Required]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
