using AuthNET.Sharing.WebApp.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

// [Important]
namespace AuthNET.Sharing.WebApp.Auth.Schemes
{
    public static class BasicAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Basic";
    }

    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
    {
    }

    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public const string AuthorizationScheme = "Basic";

        private readonly DataContext _dataContext;

        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DataContext dataContext) : base(options, logger, encoder, clock)
        {
            _dataContext = dataContext;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers.Add(HeaderNames.WWWAuthenticate, AuthorizationScheme);
            return base.HandleChallengeAsync(properties);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var request = Request;
            var authHeader = request.Headers[HeaderNames.Authorization];

            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderVal.Scheme.Equals(AuthorizationScheme, StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(authHeaderVal.Parameter))
                {
                    const string IsoCharset = "ISO-8859-1";
                    var encoding = Encoding.GetEncoding(IsoCharset);
                    string credentials = encoding.GetString(Convert.FromBase64String(authHeaderVal.Parameter));

                    int separator = credentials.IndexOf(':');
                    string username = credentials.Substring(0, separator);
                    string password = credentials.Substring(separator + 1);
                    var user = _dataContext.Users.FirstOrDefault(user => user.UserName == username && user.Password == password);

                    if (user != null)
                    {
                        var identity = new GenericIdentity(user.Id);
                        var principal = new GenericPrincipal(identity, null);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                }
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
