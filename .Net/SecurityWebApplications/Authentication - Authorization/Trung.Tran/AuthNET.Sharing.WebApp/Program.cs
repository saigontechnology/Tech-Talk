using AuthNET.Sharing.WebApp.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthNET.Sharing.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            PrepareData(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void PrepareData(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                // Roles
                var adminRole = new AppRole { Id = "1", Name = AuthConstants.RoleNames.Administrator };
                var empRole = new AppRole { Id = "2", Name = AuthConstants.RoleNames.Employee };
                dataContext.Roles.AddRange(adminRole, empRole);
                dataContext.SaveChanges();

                // Permission Groups
                var groupAdmin = new PermissionGroup
                {
                    GroupName = "Admin",
                    Records = new PermissionGroupRecord[]
                    {
                        new PermissionGroupRecord{ Permission = AuthConstants.Permissions.FullAccess }
                    }
                };
                var groupReader = new PermissionGroup
                {
                    GroupName = "Reader",
                    Records = new PermissionGroupRecord[]
                    {
                        new PermissionGroupRecord{ Permission = AuthConstants.Permissions.Read }
                    }
                };
                var groupWriter = new PermissionGroup
                {
                    GroupName = "Writer",
                    Records = new PermissionGroupRecord[]
                    {
                        new PermissionGroupRecord{ Permission = AuthConstants.Permissions.Write }
                    }
                };
                dataContext.PermissionGroups.AddRange(groupAdmin, groupReader, groupWriter);
                dataContext.SaveChanges();

                // Users
                var userA = new AppUser
                {
                    Id = "user-a",
                    ApiKey = AppHelper.GetRandomSecretKey(),
                    Password = "123123",
                    UserName = "userA",
                    Resources = new AppResource[]
                    {
                        new AppResource { Name = "User A - Resource 1" },
                        new AppResource { Name = "User A - Resource 2" },
                        new AppResource { Name = "User A - Resource 3" },
                    },
                    UserRoles = new AppUserRole[]
                    {
                        new AppUserRole { Role = adminRole },
                        new AppUserRole { Role = empRole },
                    },
                    UserPermissions = new AppUserPermission[]
                    {
                        new AppUserPermission { Permission = AuthConstants.Permissions.FullAccess },
                    }
                };

                var userB = new AppUser
                {
                    Id = "user-b",
                    ApiKey = AppHelper.GetRandomSecretKey(),
                    Password = "123123",
                    UserName = "userB",
                    Resources = new AppResource[]
                    {
                        new AppResource { Name = "User B - Resource 1" },
                        new AppResource { Name = "User B - Resource 2" },
                        new AppResource { Name = "User B - Resource 3" },
                    },
                    UserRoles = new AppUserRole[]
                    {
                        new AppUserRole { Role = empRole },
                    },
                    UserPermissions = new AppUserPermission[]
                    {
                        new AppUserPermission { Permission = AuthConstants.Permissions.Read },
                    }
                };

                dataContext.Users.AddRange(userA, userB);
                dataContext.SaveChanges();
            }
        }
    }
}
