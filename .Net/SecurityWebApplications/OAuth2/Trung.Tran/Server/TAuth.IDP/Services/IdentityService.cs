using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TAuth.Cross.Services;
using TAuth.IDP.Models;

namespace TAuth.IDP.Services
{
    public interface IIdentityService
    {
        Task SendActivationEmailAsync(AppUser user, IUrlHelper urlHelper, string requestScheme);
    }

    public class IdentityService : IIdentityService
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(IEmailService emailService,
            UserManager<AppUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task SendActivationEmailAsync(AppUser user, IUrlHelper urlHelper, string requestScheme)
        {
            // Default: 1 day timespan for token valid lifetime => need resend function
            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = urlHelper.Action(
               nameof(IdentityServerHost.Quickstart.UI.AccountController.ConfirmEmail), "Account",
               new { userId = user.Id, token = confirmToken },
               protocol: requestScheme);

            await _emailService.SendEmailAsync(user.Email,
               "Confirm your account",
               $"Please confirm your account by clicking this <a href=\"{callbackUrl}\">link</a>");
        }
    }
}
