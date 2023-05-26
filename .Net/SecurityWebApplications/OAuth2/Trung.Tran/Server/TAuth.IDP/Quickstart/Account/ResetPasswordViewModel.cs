using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class RequestResetPasswordViewModel
    {
        [Required]
        [MaxLength(250)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public RequestResetPasswordViewModel RequestResetPasswordViewModel { get; set; }

        [Required]
        [MaxLength(250)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [MaxLength(250)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public string ResetPasswordToken { get; set; }
        public bool IdentityConfirmed { get; set; }
    }
}
