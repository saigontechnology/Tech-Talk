using AuthNET.Sharing.WebApp.Auth.Policies;
using AuthNET.Sharing.WebApp.Auth.Requirements;
using AuthNET.Sharing.WebApp.Auth.Schemes;
using AuthNET.Sharing.WebApp.Persistence;
using AuthNET.Sharing.WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using static AuthNET.Sharing.WebApp.AuthConstants;

namespace AuthNET.Sharing.WebApp
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
            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase(nameof(AuthNET)));

            services.AddScoped<IIdentityService, IdentityService>();

            // [Important] Authentication

            // Default scheme for Sign-in, Sign-out, Challenge, Forbid, Authenticate ...
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

                // Cookie-based
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
                {
                    opt.LoginPath = "/Login";
                    opt.LogoutPath = "/Logout";
                    opt.AccessDeniedPath = "/AccessDenied";

                    //opt.Events = new CookieAuthenticationEvents
                    //{
                    //    OnValidatePrincipal = async (context) =>
                    //    {
                    //        var identityService = context.HttpContext.RequestServices.GetRequiredService<IIdentityService>();
                    //        var userId = context.Principal.Identity.Name;
                    //        var hasChanged = await identityService.HasUserBeenChangedSince(
                    //            userId,
                    //            context.Properties.IssuedUtc.Value);

                    //        if (hasChanged)
                    //        {
                    //            var userModel = await identityService.GetUserAsync(userId);
                    //            var userPrincipal = await identityService.GetUserPrincipalAsync(userModel,
                    //                CookieAuthenticationDefaults.AuthenticationScheme);
                    //            context.ReplacePrincipal(userPrincipal);
                    //            context.ShouldRenew = true;
                    //        }
                    //    }
                    //};
                })

                // Token-based
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.TokenValidationParameters = AuthConstants.Jwt.DefaultTokenParameters;
                })

                // Custom: Basic Authentication
                .AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(
                    BasicAuthenticationDefaults.AuthenticationScheme, opt => { })

                // Custom: ApiKey Authentication
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                    ApiKeyAuthenticationDefaults.AuthenticationScheme, opt => { })

                // 3rd party: Temporary login for 3rd party
                .AddCookie(AuthConstants.AuthenticationSchemes.ExternalCookies)

                // 3rd party: Facebook
                .AddFacebook(AuthConstants.AuthenticationSchemes.Facebook, "Facebook Login", opt =>
                {
                    // Use UsersSecret (dev only) or Environment variables to store secret, for better security
                    opt.AppId = Configuration.GetValue<string>("Facebook:AppId");
                    opt.AppSecret = Configuration.GetValue<string>("Facebook:AppSecret");
                    opt.SignInScheme = AuthConstants.AuthenticationSchemes.ExternalCookies;
                })

                // 3rd party: Google
                .AddGoogle(AuthConstants.AuthenticationSchemes.Google, "Google Login", opt =>
                {
                    // Use UsersSecret or Environment variables to configure
                    opt.ClientId = Configuration.GetValue<string>("Google:ClientId");
                    opt.ClientSecret = Configuration.GetValue<string>("Google:ClientSecret");
                    opt.SignInScheme = AuthConstants.AuthenticationSchemes.ExternalCookies;
                });

            // [Important] Next sharing session
            // OAuth2
            //.AddOAuth(AuthConstants.AuthenticationSchemes.OAuth2, opt => { })
            // OpenIdConnect
            //.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt => { });

            // [Important] Authorization
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(AuthConstants.Policies.CanAccessAdminArea, opt =>
                {
                    opt.RequireAuthenticatedUser()
                        .RequireRole(AuthConstants.RoleNames.Employee)
                        .RequireRole(AuthConstants.RoleNames.Administrator);
                });

                opt.AddPolicy(AuthConstants.Policies.CanReadResource, opt =>
                {
                    opt.RequireAuthenticatedUser()
                        .RequireClaim(AuthConstants.AppClaimTypes.Permission,
                            AuthConstants.Permissions.FullAccess,
                            AuthConstants.Permissions.Read);
                });

                opt.AddPolicy(AuthConstants.Policies.CanWriteResource, opt =>
                {
                    opt.RequireAuthenticatedUser()
                        .RequireClaim(AuthConstants.AppClaimTypes.Permission,
                            AuthConstants.Permissions.FullAccess,
                            AuthConstants.Permissions.Write);
                });

                opt.AddPolicy(AuthConstants.Policies.SingleRoleOnly, opt =>
                {
                    opt.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.FindAll(ClaimTypes.Role).Count() == 1);
                });

                opt.AddPolicy(AuthConstants.Policies.CanManageResource, opt =>
                {
                    opt.RequireAuthenticatedUser()
                        .AddRequirements(new IsResourceOwnerOrRolesRequirement(AuthConstants.RoleNames.Administrator));
                });

                #region Policy based
                opt.AddPolicy(AuthConstants.Policies.UserNameContains_A, opt =>
                {
                    opt.RequireAuthenticatedUser()
                        .AddRequirements(new UserNameContainsRequirement("A"));
                });

                opt.AddPolicy(AuthConstants.Policies.UserNameContains_B, opt =>
                {
                    opt.RequireAuthenticatedUser()
                        .AddRequirements(new UserNameContainsRequirement("B"));
                });
                #endregion
            });

            // [Important] Scan all authorization handlers to register into container
            var allAuthHandlers = typeof(Startup).Assembly.GetTypes()
                .Where(type => typeof(IAuthorizationHandler).IsAssignableFrom(type)).ToArray();

            foreach (var handler in allAuthHandlers)
                services.AddScoped(typeof(IAuthorizationHandler), handler);

            // [Important] Un-comment to use Application custom policy provider
            services.AddSingleton<IAuthorizationPolicyProvider, ApplicationPolicyProvider>();

            // [Important] Global authorization config
            services.AddRazorPages(opt =>
            {
                opt.Conventions
                    .AuthorizeFolder("/")
                    .AuthorizeFolder("/Admin", AuthConstants.Policies.CanAccessAdminArea)
                    .AllowAnonymousToPage("/Login");

                #region Policies manager
                opt.Conventions
                    .AuthorizePage("/Auth/UserNameContainsA",
                        $"{AuthConstants.Policies.UserNameContains}_A")
                    .AuthorizePage("/Auth/Special",
                        $"{AuthConstants.Policies.UserNameContains}_A;" +
                        $"{AuthConstants.Policies.UserNameContains}_user;" +
                        $"{AuthConstants.Policies.HasRole}_{AuthConstants.RoleNames.Employee}");
                // more and more
                #endregion
            });
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AuthNET.Sharing",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                });

                const string ApplicationApiKey = nameof(ApplicationApiKey);
                const string ApplicationBasic = nameof(ApplicationBasic);

                c.AddSecurityDefinition(ApplicationApiKey,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter an Authorization Header value",
                        Name = HeaderNames.Authorization,
                        Type = SecuritySchemeType.ApiKey,
                    });

                c.AddSecurityDefinition(ApplicationBasic,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a credentials",
                        Name = HeaderNames.Authorization,
                        Type = SecuritySchemeType.Http,
                        Scheme = BasicAuthenticationDefaults.AuthenticationScheme
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApplicationApiKey
                            }
                        },
                        new string[0]
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApplicationBasic
                            }
                        },
                        new string[0]
                    }
                });
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthNET.Sharing API V1");
                c.RoutePrefix = "swagger";
            });

            app.UseAuthentication(); // [Important] Authentication middleware

            // [Important] Debugging user middleware
            app.Use(async (context, next) =>
            {
                var user = context.User;
                await next();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages(); // [Important] RequireAuthorization
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
