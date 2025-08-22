using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Identity.Application.ClientAccounts.Commands.CommandResults;
using PlanningBook.Identity.Application.Helpers.Interfaces;
using PlanningBook.Identity.Application.Providers.Interfaces;
using PlanningBook.Identity.Infrastructure;
using PlanningBook.Identity.Infrastructure.Entities;
using PlanningBook.Repository.EF;

namespace PlanningBook.Identity.Application.ClientAccounts.Commands
{
    #region Command Model
    public sealed class SignInClientAccountCommand : ICommand<CommandResult<SignInClientAccountCommandResult>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public SignInClientAccountCommand(string username, string password)
        {
            UserName = username;
            Password = password;
        }

        public ValidationResult GetValidationResult()
        {
            var invalid = string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password);
            if (invalid)
                return ValidationResult.Failure(null, null);

            return ValidationResult.Success();
        }
    }
    #endregion Command Model

    #region Command Handler
    public sealed class SignInClientAccountCommandHandler(
        UserManager<Account> _userManager,
        SignInManager<Account> _signInManager,
        IConfiguration _configuration,
        IEFClassRepository<PBIdentityDbContext, AccountToken, Guid> _accountTokenRepository,
        IPasswordHasher _passwordHasher,
        ITokenProvider _tokenProvider)
        : ICommandHandler<SignInClientAccountCommand, CommandResult<SignInClientAccountCommandResult>>
    {
        public async Task<CommandResult<SignInClientAccountCommandResult>> HandleAsync(SignInClientAccountCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null || !command.GetValidationResult().IsValid)
                return CommandResult<SignInClientAccountCommandResult>.Failure(null, null);

            var accountExisted = await _userManager.Users
                .FirstOrDefaultAsync(account => account.NormalizedUserName.Equals(command.UserName.ToUpper()));

            // TODO: Seperate error & log
            if (accountExisted == null || !accountExisted.IsActive || accountExisted.IsDeleted)
                return CommandResult<SignInClientAccountCommandResult>.Failure(null, null);

            var validPassword = _passwordHasher.Verify(command.Password, accountExisted.PasswordHash);
            if (!validPassword)
                return CommandResult<SignInClientAccountCommandResult>.Failure(null, null);

           //await _signInManager.SignInAsync(accountExisted, true);
            //var signInResult = await _signInManager.PasswordSignInAsync(command.UserName, accountExisted.PasswordHash, true, false);
            //if (!signInResult)
            //    return CommandResult<SignInClientAccountCommandResult>.Failure(null, null);

            var token = _tokenProvider.GenerateToken(accountExisted);
            var refreshToken = _tokenProvider.GenerateRefreshToken();

            // TODO: Should not delete => Inplement revoke token & refesh token later
            //await _accountTokenRepository.HardDeleteAsync(x => x.AccountId == accountExisted.Id, cancellationToken);

            var configExpirationDays = _configuration.GetValue<int>("Jwt:RefreshTokenExpirationInDays", 7);
            // TODO: Need To Fix later
            //var newToken = new AccountToken()
            //{
            //    Name = $"{accountExisted.Id}-{DateTime.UtcNow.ToString()}",
            //    AccountId = accountExisted.Id,
            //    // TODO: Need some constant for LoginProvider
            //    LoginProvider = "InternalIdentitySystem",
            //    Token = token,
            //    RefreshToken = refreshToken,
            //    RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(configExpirationDays),
            //    IsRevoked = false
            //};
            //await _accountTokenRepository.AddAsync(newToken, cancellationToken);

            //// TODO: Should re-implement unitOfWork for case save change many different tables
            //await _accountTokenRepository.SaveChangeAsync();

            return CommandResult<SignInClientAccountCommandResult>.Success(new SignInClientAccountCommandResult()
            {
                UserId = accountExisted.Id,
                Token = token,
                RefreshToken = refreshToken
            });
        }
    }
    #endregion Command Handler
}
