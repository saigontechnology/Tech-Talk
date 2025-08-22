using PlanningBook.Domain;
using PlanningBook.Domain.Interfaces;
using PlanningBook.Repository.EF;
using PlanningBook.Users.Infrastructure;
using PlanningBook.Users.Infrastructure.Entites;

namespace PlanningBook.Users.Application.Persons.Commands
{
    #region Command Model
    public sealed class CreatePersonCommand : ICommand<CommandResult<Guid>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public CreatePersonCommand(string? firstName, string? lastName, string? email, string? phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public ValidationResult GetValidationResult()
        {
            return ValidationResult.Success();
        }
    }
    #endregion Command Model

    #region Command Handler
    public sealed class CreatePersonCommandHandler(IEFRepository<PBPersonDbContext, Person, Guid> _personRepository) : ICommandHandler<CreatePersonCommand, CommandResult<Guid>>
    {
        public async Task<CommandResult<Guid>> HandleAsync(CreatePersonCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null || !command.GetValidationResult().IsValid)
                return CommandResult<Guid>.Failure(null, null);

            var person = new Person()
            {
                FirstName = command?.FirstName,
                LastName = command?.LastName,
                Email = command?.Email,
                PhoneNumber = command?.PhoneNumber
            };

            await _personRepository.AddAsync(person, cancellationToken);
            await _personRepository.SaveChangeAsync(cancellationToken);

            return CommandResult<Guid>.Success(person.Id);
        }
    }
    #endregion Command Handler
}
