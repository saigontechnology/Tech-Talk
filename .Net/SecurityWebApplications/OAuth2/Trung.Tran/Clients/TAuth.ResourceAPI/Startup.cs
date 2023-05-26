using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAuth.Cross;
using TAuth.Resource.Cross;
using TAuth.ResourceAPI.Auth.Policies;
using TAuth.ResourceAPI.Entities;
using TAuth.ResourceAPI.Services;

namespace TAuth.ResourceAPI
{
    public class Startup
    {
        public static AppSettings AppSettings { get; } = new AppSettings();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Configuration.Bind(nameof(AppSettings), AppSettings);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<ResourceContext>(opt =>
                opt.UseSqlite(Configuration.GetConnectionString(nameof(ResourceContext))));

            services.AddScoped<IUserProvider, UserProvider>();

            // Use classic JWT Bearer
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //    {
            //        opt.Authority = AppSettings.IdpUrl;
            //        opt.Audience = "resource_api";
            //        opt.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            NameClaimType = JwtClaimTypes.Name,
            //            RoleClaimType = JwtClaimTypes.Role
            //        };
            //    });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                // JWT tokens
                .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, opt =>
                {
                    opt.Authority = AppSettings.IdpUrl;
                    opt.Audience = "resource_api";
                    opt.TokenValidationParameters.ClockSkew = TimeSpan.Zero; // demo only: since local will not have clock skew
                })
                // Reference tokens
                .AddOAuth2Introspection(OpenIdConnectConstants.AuthSchemes.Introspection, opt =>
                {
                    opt.Authority = AppSettings.IdpUrl;
                    opt.ClientId = "resource_api";
                    opt.ClientSecret = "resource-api-secret";
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(PolicyNames.Resource.CanDeleteResource, builder => builder
                    .AddRequirements(new DeleteResourceRequirement()
                    {
                        TestName = "Test"
                    }));

                opt.AddPolicy(PolicyNames.IsOwner, builder => builder.AddRequirements(new IsOwnerRequirement()));

                opt.AddPolicy(PolicyNames.IsLucky, builder => builder.RequireAssertion(context => DateTime.UtcNow.Ticks % 10 > 3));

                opt.AddPolicy(PolicyNames.WorkerOnly, builder => builder.RequireAuthenticatedUser().RequireScope("resource_api.background"));

                opt.AddPolicy(PolicyNames.IsAdmin, builder => builder.RequireRole(RoleNames.Administrator));
            });

            var allAuthHandlers = typeof(Startup).Assembly.GetTypes()
                .Where(type => typeof(IAuthorizationHandler).IsAssignableFrom(type)).ToArray();

            foreach (var handler in allAuthHandlers)
                services.AddSingleton(typeof(IAuthorizationHandler), handler);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                });

                #region Bearer/Api Key
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = HeaderNames.Authorization,
                        Type = SecuritySchemeType.ApiKey
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[0]
                    }
                });
                #endregion
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                var path = context.Request.Path;
                var query = context.Request.Query;
                var form = context.Request.HasFormContentType ? context.Request.Form : null;
                var method = context.Request.Method;
                var headers = context.Request.Headers;
                string body = null;
                if (method.Equals("post", StringComparison.OrdinalIgnoreCase) && !context.Request.HasFormContentType)
                {
                    // Leave the body open so the next middleware can read it.
                    using (var reader = new StreamReader(
                        context.Request.Body,
                        encoding: Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: false,
                        bufferSize: 1028,
                        leaveOpen: true))
                    {
                        body = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;
                    }
                }

                Console.WriteLine("=========== REQUEST =============");
                Console.WriteLine($"Method: {method}");
                Console.WriteLine($"Path: {path}");
                Console.WriteLine($"Query: {JsonConvert.SerializeObject(query, Formatting.Indented)}");
                Console.WriteLine($"Form: {JsonConvert.SerializeObject(form, Formatting.Indented)}");
                Console.WriteLine($"Headers: {JsonConvert.SerializeObject(headers, Formatting.Indented)}");
                Console.WriteLine($"Body: {body}");
                Console.WriteLine("========= END REQUEST ===========");

                await LogResponseAsync(context, next);
            });

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                //builder.AllowAnyOrigin();
                builder.SetIsOriginAllowed(origin =>
                {
                    return true;
                });
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //.RequireAuthorization();
            });
        }

        static async Task LogResponseAsync(HttpContext context, Func<Task> next)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await next();

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);

            Console.WriteLine("=========== RESPONSE =============");
            Console.WriteLine($"Status: {context.Response.StatusCode}");
            Console.WriteLine($"Headers: {JsonConvert.SerializeObject(context.Response.Headers)}");
            if (context.Response.ContentType?.Contains("json") == true)
            {
                Console.WriteLine($"Body: {text}");
            }
            Console.WriteLine("========= END RESPONSE ===========");
        }
    }
}
