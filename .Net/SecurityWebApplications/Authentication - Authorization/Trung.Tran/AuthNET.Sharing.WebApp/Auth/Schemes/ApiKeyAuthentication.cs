using AuthNET.Sharing.WebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

// [Important]
namespace AuthNET.Sharing.WebApp.Auth.Schemes
{
    public static class ApiKeyAuthenticationDefaults
    {
        public const string AuthenticationScheme = "ApiKey";
    }

    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
    }

    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        public const string AuthorizationScheme = "Bearer";

        private readonly IIdentityService _identityService;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IIdentityService identityService) : base(options, logger, encoder, clock)
        {
            _identityService = identityService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var request = Request;
            var authHeader = request.Headers[HeaderNames.Authorization];

            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderVal.Scheme.Equals(AuthorizationScheme, StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(authHeaderVal.Parameter))
                {
                    string apiKey = authHeaderVal.Parameter;
                    var user = await _identityService.AuthenticateAsync(apiKey);

                    if (user != null)
                    {
                        var identity = new GenericIdentity(user.Id);
                        var principal = new GenericPrincipal(identity, null);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return AuthenticateResult.Success(ticket);
                    }
                }
            }

            return AuthenticateResult.NoResult();
        }
    }
}
