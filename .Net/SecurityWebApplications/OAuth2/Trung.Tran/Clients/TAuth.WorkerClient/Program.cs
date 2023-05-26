using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System;

namespace TAuth.WorkerClient
{
    public class Program
    {
        const string AppsettingsJsonFile = "appsettings.json";

        public static AppSettings AppSettings { get; } = new AppSettings();

        private IServiceProvider _serviceProvider;
        private IConfigurationRoot _configurationRoot;

        private void Setup()
        {
            _configurationRoot = new ConfigurationBuilder()
                .AddJsonFile(AppsettingsJsonFile)
                .Build();

            _configurationRoot.Bind(nameof(AppSettings), AppSettings);

            var services = new ServiceCollection();

            services.AddHttpClient<IWorkerService, WorkerService>(opt =>
            {
                opt.BaseAddress = new Uri(AppSettings.ResourceApiUrl);
            }).AddClientAccessTokenHandler();

            services.AddHttpClient<IIdentityService, IdentityService>(opt =>
            {
                opt.BaseAddress = new Uri(AppSettings.IdpUrl);
            }).AddClientAccessTokenHandler();

            services.AddAuthentication(opt =>
            {
                opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
            {
                opt.Authority = AppSettings.IdpUrl;
                opt.ClientId = "worker-client-id";
                opt.ResponseType = OpenIdConnectResponseType.Token;
                opt.Scope.Add("resource_api.background");
                opt.ClientSecret = "worker-client-secret";

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });

            // adds user and client access token management
            services.AddAccessTokenManagement(options =>
            {
                // client config is inferred from OpenID Connect settings
                // if you want to specify scopes explicitly, do it here, otherwise the scope parameter will not be sent
                options.Client.Scope = "resource_api.background";
            }).ConfigureBackchannelHttpClient()
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            }));

            _serviceProvider = services.BuildServiceProvider();
        }

        public void Start()
        {
            Setup();
            using var scope = _serviceProvider.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<IWorkerService>();
            worker.StartAsync().Wait();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            program.Start();
        }
    }
}
