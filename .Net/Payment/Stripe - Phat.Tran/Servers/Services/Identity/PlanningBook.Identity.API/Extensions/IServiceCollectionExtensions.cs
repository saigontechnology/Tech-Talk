using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Identity.Application.Accounts.Commands;
using PlanningBook.Identity.Application.ClientAccounts.Commands;
using PlanningBook.Identity.Application.ClientAccounts.Commands.CommandResults;
using PlanningBook.Identity.Application.Helpers;
using PlanningBook.Identity.Application.Helpers.Interfaces;
using PlanningBook.Identity.Application.Providers;
using PlanningBook.Identity.Application.Providers.Interfaces;
using PlanningBook.Repository.EF;

namespace PlanningBook.Identity.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Add Services & Helpers
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenProvider, TokenProvider>();
            #endregion Add Services & Helpers

            #region Add Repositories
            services.AddScoped(typeof(IEFRepository<,,>), typeof(EFRepository<,,>));
            services.AddScoped(typeof(IEFRepository<,,,>), typeof(EFRepository<,,,>));
            services.AddScoped(typeof(IEFClassRepository<,,>), typeof(EFClassRepository<,,>));
            services.AddScoped(typeof(IEFClassRepository<,,,>), typeof(EFClassRepository<,,,>));
            #endregion Add Repositories

            return services;
        }

        public static IServiceCollection RegistryCommandQueryExecutor(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICommandExecutor, CommandExecutor>();
            services.AddTransient<IQueryExecutor, QueryExecutor>();
            return services;
        }

        public static IServiceCollection RegistryAccountModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICommandHandler<ChangePasswordClientAccountCommand, CommandResult<bool>>, ChangePasswordClientAccountCommandHandler>();
            services.AddScoped<ICommandHandler<SignInClientAccountCommand, CommandResult<SignInClientAccountCommandResult>>, SignInClientAccountCommandHandler>();
            services.AddScoped<ICommandHandler<SignOutClientAccountCommand, CommandResult<bool>>, SignOutClientAccountCommandHandler>();
            services.AddScoped<ICommandHandler<SignUpClientAccountCommand, CommandResult<Guid>>, SignUpClientAccountCommandHandler>();

            return services;
        }
    }
}
