// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Threading.Tasks;
using TAuth.IDP.Models;

namespace TAuth.IDP
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\identityserver.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                Log.Information("Starting host...");
                var host = CreateHostBuilder(args).Build();
                InitAsync(host).Wait();
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task InitAsync(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<IdpContext>();

            await context.Database.MigrateAsync();

            if (!await context.Clients.AnyAsync())
            {
                var clients = Config.Clients.Select(o => o.ToEntity()).ToArray();
                await context.Clients.AddRangeAsync(clients);
                await context.SaveChangesAsync();
            }

            if (!await context.IdentityResources.AnyAsync())
            {
                var identityResources = Config.IdentityResources.Select(o => o.ToEntity()).ToArray();
                await context.IdentityResources.AddRangeAsync(identityResources);
                await context.SaveChangesAsync();
            }

            if (!await context.ApiScopes.AnyAsync())
            {
                var apiScopes = Config.ApiScopes.Select(o => o.ToEntity()).ToArray();
                await context.ApiScopes.AddRangeAsync(apiScopes);
                await context.SaveChangesAsync();
            }

            if (!await context.ApiResources.AnyAsync())
            {
                var apiResources = Config.ApiResources.Select(o => o.ToEntity()).ToArray();
                await context.ApiResources.AddRangeAsync(apiResources);
                await context.SaveChangesAsync();
            }

            if (!await context.Users.AnyAsync())
            {
                var users = TestUsers.Users.Select(o =>
                {
                    var emailConfirmed = bool.Parse(o.Claims.FirstOrDefault(o => o.Type == JwtClaimTypes.EmailVerified).Value);
                    return new
                    {
                        Password = o.Password,
                        User = new AppUser()
                        {
                            Id = o.SubjectId,
                            Email = o.Claims.FirstOrDefault(o => o.Type == JwtClaimTypes.Email).Value,
                            EmailConfirmed = emailConfirmed,
                            Active = emailConfirmed,
                            UserName = o.Username
                        },
                        o.Claims
                    };
                }).ToArray();

                foreach (var userInfo in users)
                {
                    var result = await userManager.CreateAsync(userInfo.User, userInfo.Password);

                    if (!result.Succeeded)
                        throw new Exception("Failed to create user");

                    var roles = userInfo.Claims.Where(o => o.Type == JwtClaimTypes.Role).Select(o => o.Value).ToArray();

                    if (roles.Length > 0)
                    {
                        var existedRoles = await context.Roles.Select(o => o.Name).ToArrayAsync();
                        var newRoles = roles.Where(r => !existedRoles.Contains(r)).ToArray();

                        foreach (var role in newRoles)
                        {
                            result = await roleManager.CreateAsync(new IdentityRole(role));

                            if (!result.Succeeded)
                                throw new Exception("Failed to create role");
                        }

                        result = await userManager.AddToRolesAsync(userInfo.User, roles);

                        if (!result.Succeeded)
                            throw new Exception("Failed to add user to roles");
                    }

                    result = await userManager.AddClaimsAsync(userInfo.User, userInfo.Claims);

                    if (!result.Succeeded)
                        throw new Exception("Failed to add claims for user");
                }
            }
        }
    }
}