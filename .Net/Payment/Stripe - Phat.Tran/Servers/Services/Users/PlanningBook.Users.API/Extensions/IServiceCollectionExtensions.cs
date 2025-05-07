using PlanningBook.Domain.Interfaces;
using PlanningBook.Domain;
using PlanningBook.Repository.EF;
using PlanningBook.Users.Application.Persons.Commands;

namespace PlanningBook.Users.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
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

        public static IServiceCollection RegistryPersonModule(this IServiceCollection services, IConfiguration configuration)
        {

            #region Commands
            services.AddScoped<ICommandHandler<CreatePersonCommand, CommandResult<Guid>>, CreatePersonCommandHandler>();
            #endregion Commands

            return services;
        }
    }
}
