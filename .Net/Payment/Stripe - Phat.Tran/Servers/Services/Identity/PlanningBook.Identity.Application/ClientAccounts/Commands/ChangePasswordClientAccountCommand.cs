using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Identity.Infrastructure.Entities;

namespace PlanningBook.Identity.Application.ClientAccounts.Commands
{
    #region Command Model
    public sealed class ChangePasswordClientAccountCommand : ICommand<CommandResult<bool>>
    {
        public Guid? UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public ChangePasswordClientAccountCommand(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public ValidationResult GetValidationResult()
        {
            var invalid = string.IsNullOrWhiteSpace(OldPassword) ||
                string.IsNullOrWhiteSpace(NewPassword) ||
                UserId == Guid.Empty ||
                UserId == null;

            if (invalid)
                return ValidationResult.Failure(null, new List<string>() { "Changes Password Failed!" });

            return ValidationResult.Success();
        }
    }
    #endregion Command Model

    #region Command Handler
    public sealed class ChangePasswordClientAccountCommandHandler(UserManager<Account> _userManager)
        : ICommandHandler<ChangePasswordClientAccountCommand, CommandResult<bool>>
    {
        public async Task<CommandResult<bool>> HandleAsync(ChangePasswordClientAccountCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null || !command.GetValidationResult().IsValid)
                // TODO: Log Error
                return CommandResult<bool>.Failure(null, null);

            var userExisted = await _userManager.Users
                    .FirstOrDefaultAsync(account => account.Id == command.UserId &&
                        account.IsActive &&
                        !account.IsDeleted, cancellationToken);

            if (userExisted == null)
                return CommandResult<bool>.Failure(null, null);

            var result = await _userManager.ChangePasswordAsync(userExisted, command.OldPassword, command.NewPassword);

            return CommandResult<bool>.Success(result.Succeeded);
        }
    }
    #endregion Command Handler
}
