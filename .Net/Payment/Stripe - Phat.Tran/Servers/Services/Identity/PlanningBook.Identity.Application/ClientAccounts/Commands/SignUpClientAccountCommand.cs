using Microsoft.AspNetCore.Identity;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Identity.Infrastructure.Entities;
using PlanningBook.Domain;
using Microsoft.EntityFrameworkCore;
using PlanningBook.Identity.Application.Helpers.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace PlanningBook.Identity.Application.Accounts.Commands
{
    #region Command Model
    public sealed class SignUpClientAccountCommand : ICommand<CommandResult<Guid>>
    {
        public string Username { get; set; }
        public string Password { get; set; } // TO DO: Should not plain text from FE -> BE
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public SignUpClientAccountCommand(string username, string password, string? email, string? phoneNumber)
        {
            Username = username;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public ValidationResult GetValidationResult()
        {
            var invalid = string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password);
            if (invalid)
                return ValidationResult.Failure(null, new List<string>()
                    {
                        "Register Account Failed: Invalid request!"
                    });

            return ValidationResult.Success();
        }
    }
    #endregion Command Model

    #region Command Handler
    public sealed class SignUpClientAccountCommandHandler(
        UserManager<Account> _accountManager,
        IPasswordHasher _passwordHasher,
        IHttpClientFactory _httpClientFactory
        )
        : ICommandHandler<SignUpClientAccountCommand, CommandResult<Guid>>
    {
        public async Task<CommandResult<Guid>> HandleAsync(SignUpClientAccountCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null || !command.GetValidationResult().IsValid)
            {
                // TODO: Log Error
                // TODO: Handle throw error with error message
                return CommandResult<Guid>.Failure(null, null);
            }

            //var accountExisted = await _accountManager.FindByEmailAsync(command.Email);
            //var accountExisted = await _accountManager.Users
            //    .FirstOrDefaultAsync(x => x.UserName == command.Username &&
            //    x.);

            var tempEmail = command?.Email ?? $"{command.Username}@mail.com";
            if (!string.IsNullOrWhiteSpace(tempEmail))
            {
                var isEmailUsed = await _accountManager.Users
                    .AnyAsync(account => account.Email == tempEmail &&
                        account.IsActive &&
                        !account.IsDeleted, cancellationToken);
                if (isEmailUsed)
                {
                    // TODO: Log Error & Return Error
                    return CommandResult<Guid>.Failure(null, null);
                }
            }

            if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
            {
                var isPhoneUsed = await _accountManager.Users
                    .AnyAsync(account => account.PhoneNumber == command.PhoneNumber &&
                        account.IsActive &&
                        !account.IsDeleted, cancellationToken);

                if (isPhoneUsed)
                {
                    // TODO: Log Error & Return Error
                    return CommandResult<Guid>.Failure(null, null);
                }
            }

            var accountExisted = await _accountManager.Users
                .AnyAsync(account => account.UserName == command.Username
                && account.Email == tempEmail
                && account.PhoneNumber == command.PhoneNumber
                && account.IsActive
                && !account.IsDeleted, cancellationToken);

            if (accountExisted)
            {
                // TODO: Log Error
                //TODO: Improve for flow SSO (with Link account)
                // Example User craete an account username/password with email A
                // After that the login by Gmail with same email A
                // => Ask to login by username/passwork and link Gmail in account setting
                return CommandResult<Guid>.Failure(null, null);
            }

            var passwordHash = _passwordHasher.Hash(command.Password);
            var account = new Account()
            {
                UserName = command.Username,
                NormalizedUserName = command.Username.ToUpper(),
                Email = tempEmail,
                NormalizedEmail = tempEmail.ToUpper(),
                PhoneNumber = command?.PhoneNumber ?? null,
                PasswordHash = passwordHash,
                IsDeleted = false,
                IsActive = true
            };

            var result = await _accountManager.CreateAsync(account);
            if (!result.Succeeded)
            {
                return CommandResult<Guid>.Failure(null, result.Errors.ToString());
            }

            // TODO: Send Confirmed email & sms
            // TODO: Send to Person Service API to create Person record
            account.PasswordHash = passwordHash;
            await _accountManager.UpdateAsync(account);

            #region Create Person
            //TODO: Should use bus to handler
            var client = _httpClientFactory.CreateClient("Person");
            var jsonContent = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("ExposurePerson/Create", jsonContent);

            if(response.IsSuccessStatusCode)
            {
                var test = 1;
            }
            #endregion Create Person

            return CommandResult<Guid>.Success(account.Id);
        }
    }
    #endregion Command Handler
}
