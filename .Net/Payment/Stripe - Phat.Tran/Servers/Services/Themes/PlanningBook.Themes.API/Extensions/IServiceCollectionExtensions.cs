using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Themes.Application.Domain.Invoices.Commands;
using PlanningBook.Themes.Application.Domain.Invoices.Commands.Models;
using PlanningBook.Themes.Application.Domain.Invoices.Queries;
using PlanningBook.Themes.Application.Domain.Invoices.Queries.Models;
using PlanningBook.Themes.Application.Domain.StripeCustomers.Commands;
using PlanningBook.Themes.Application.Domain.StripeCustomers.Queries;
using PlanningBook.Themes.Infrastructure.Entities;
using Stripe;

namespace PlanningBook.Themes.API.Extensions
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

        public static IServiceCollection RegistryThemeModule(this IServiceCollection services, IConfiguration configuration)
        {
            #region Commands
            // Invoices
            services.AddScoped<ICommandHandler<CreateInvoiceCommand, CommandResult<CreateInvoiceCommandResult>>, CreateInvoiceCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateInvoiceCommand, CommandResult<Guid>>, UpdateInvoiceCommandHandler>();

            // Stripe Customer - Payment Method
            services.AddScoped<ICommandHandler<CreateStripeCustomerCommand, CommandResult<string>>, CreateStripeCustomerCommandHandler>();
            services.AddScoped<ICommandHandler<CreateCustomerPaymentMethodCommand, CommandResult<string>>, CreateCustomerPaymentMethodCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateStripeCustomerCommand, CommandResult<StripeCustomer>>, UpdateStripeCustomerCommandHandler>();
            services.AddScoped<ICommandHandler<CancelSubscriptionCommand, CommandResult<bool>>, CancelSubscriptionCommandHandler>();
            services.AddScoped<ICommandHandler<ResumeSubscriptionCommand, CommandResult<bool>>, ResumeSubscriptionCommandHandler>();
            services.AddScoped<ICommandHandler<CreatePaymentIntentCommand, CommandResult<bool>>, CreatePaymentIntentCommandHandler>();
            #endregion Commands

            #region Queries
            // Invoices
            services.AddScoped<IQueryHandler<GetUserInvoicesQuery, QueryResult<List<UserInvoiceModel>>>, GetUserInvoicesQueryHandler>();

            // Stripe Customer - Payment method
            services.AddScoped<IQueryHandler<GetUserStripeCustomerQuery, QueryResult<StripeCustomer>>, GetUserStripeCustomerQueryHandler>();
            #endregion Queries
            return services;
        }
    }
}
