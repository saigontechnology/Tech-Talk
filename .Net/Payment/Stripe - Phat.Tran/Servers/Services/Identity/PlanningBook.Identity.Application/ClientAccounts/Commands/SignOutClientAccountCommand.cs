using Microsoft.AspNetCore.Identity;
using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Identity.Infrastructure;
using PlanningBook.Identity.Infrastructure.Entities;
using PlanningBook.Repository.EF;

namespace PlanningBook.Identity.Application.ClientAccounts.Commands
{
    // TODO: Implement
    #region Command Model
    public sealed class SignOutClientAccountCommand : ICommand<CommandResult<bool>>
    {
        public Guid? AccountId { get; set; }
        public string? Token { get; set; }
        public ValidationResult GetValidationResult()
        {
            var invalid = AccountId == Guid.Empty || string.IsNullOrWhiteSpace(Token);
            if (invalid)
                return ValidationResult.Failure(null, null);

            return ValidationResult.Success();
        }
    }
    #endregion Command Model

    #region Command Handler
    // TODO: Create token & refresh token revoke Service
    public sealed class SignOutClientAccountCommandHandler(
        SignInManager<Account> _signInManager,
        UserManager<Account> _accountManager,
        IEFClassRepository<PBIdentityDbContext, AccountToken, Guid> _accountTokenRepository,
        IEFRepository<PBIdentityDbContext, RevokedToken, string> _revokedTokenRepository,
        PBIdentityDbContext _pBIdentityDbContext)
        : ICommandHandler<SignOutClientAccountCommand, CommandResult<bool>>
    {
        public async Task<CommandResult<bool>> HandleAsync(SignOutClientAccountCommand command, CancellationToken cancellationToken = default)
        {
            if (!command.GetValidationResult().IsValid)
                return CommandResult<bool>.Failure(null, null);

            var tokenExisted = await _accountTokenRepository
                .GetFirstAsync(x => x.AccountId == command.AccountId &&
                                    x.Token == command.Token, cancellationToken);

            if (tokenExisted == null || tokenExisted.IsRevoked)
            {
                // TODO: Log error
                return CommandResult<bool>.Success(true);
            }

            tokenExisted.IsRevoked = true;
            await _accountTokenRepository.UpdateAsync(tokenExisted, cancellationToken);
            //await _accountTokenRepository.SaveChangeAsync();

            var revokedToken = new RevokedToken()
            {
                Id = tokenExisted.Token
            };
            await _revokedTokenRepository.AddAsync(revokedToken, cancellationToken);

            await _signInManager.SignOutAsync();

            await _pBIdentityDbContext.SaveChangesAsync(cancellationToken);

            return CommandResult<bool>.Success(true);
        }
    }
    #endregion Comamnd Handler
}
