using System.ComponentModel.DataAnnotations;
using TAuth.IDP;

namespace IdentityServerHost.Quickstart.UI
{
    public class CheckOTPViewModel
    {
        [Required]
        [StringLength(AuthConstants.Mfa.OTPLength)]
        public string OTPCode { get; set; }

        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
