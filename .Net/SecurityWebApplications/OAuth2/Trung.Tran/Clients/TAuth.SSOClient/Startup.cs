using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TAuth.Cross;

namespace TAuth.SSOClient
{
    public class Startup
    {
        public static AppSettings AppSettings { get; } = new AppSettings();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.Bind(nameof(AppSettings), AppSettings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(AppSettings.DataProtectionKeyPath))
                .SetApplicationName(nameof(TAuth));

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                opt.RequireAuthenticatedSignIn = false;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
                {
                    opt.Cookie.Name = CookieConstants.SharedCookieName;
                    opt.Cookie.Domain = AppSettings.SharedCookieDomain;

                    opt.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = async (context) =>
                        {
                            var additionalAuthResult = await context.HttpContext.AuthenticateAsync(
                                AuthConstants.AuthenticationSchemes.ExtraScheme);
                            var principal = additionalAuthResult.Principal;

                            if (!additionalAuthResult.Succeeded)
                            {
                                // demo only, need to try sign-in in real life scenario
                                var extraEdentity = new ClaimsIdentity();
                                extraEdentity.AddClaim(new Claim("SSO.ExtraClaim", "This is an extra claim when using shared cookie"));
                                principal = new ClaimsPrincipal(extraEdentity);
                                await context.HttpContext.SignInAsync(AuthConstants.AuthenticationSchemes.ExtraScheme,
                                    principal);
                            }

                            context.Principal.AddIdentities(principal.Identities);
                        }
                    };
                })
                .AddCookie(AuthConstants.AuthenticationSchemes.ExtraScheme, opt =>
                {
                    opt.Cookie.Name = AuthConstants.Cookie.ExtraSchemeCookieName;
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
                {
                    opt.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProvider = async context =>
                        {
                            await context.HttpContext.SignOutAsync(AuthConstants.AuthenticationSchemes.ExtraScheme);
                        },
                        OnTicketReceived = context =>
                        {
                            // [Important] avoid overwriting the shared cookie, return to home page instead
                            context.HandleResponse();
                            // demo get access token for further usage
                            var accessToken = context.Properties.GetTokenValue(OpenIdConnectConstants.PropertyNames.AccessToken);
                            context.Response.Redirect("/");
                            return Task.CompletedTask;
                        }
                    };

                    opt.Authority = AppSettings.IdpUrl;

                    // [Important] Ignore SSL for development only
                    var customHandler = new HttpClientHandler();
                    customHandler.ServerCertificateCustomValidationCallback = (_, _1, _2, _3) => true;
                    opt.BackchannelHttpHandler = customHandler;

                    opt.ClientId = "sso-client-id";
                    opt.ResponseType = OpenIdConnectResponseType.Code;
                    //opt.UsePkce = false;
                    opt.Scope.Add("email");
                    opt.Scope.Add("address");
                    opt.SaveTokens = true; // HttpContext.GetTokenAsync
                    opt.ClientSecret = "sso-client-secret";
                    opt.GetClaimsFromUserInfoEndpoint = true;

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role
                    };
                });

            services.AddRazorPages(opt =>
            {
                opt.Conventions.AuthorizeFolder("/");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
