using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Quickstart.UI
{
    public class SetupOTPAppViewModel
    {
        [Required]
        public string SecretKey { get; set; }
        public string QrCodeSetupImageUrl { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
