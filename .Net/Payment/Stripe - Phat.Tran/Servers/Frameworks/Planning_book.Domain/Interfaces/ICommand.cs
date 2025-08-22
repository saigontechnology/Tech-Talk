namespace PlanningBook.Domain.Interfaces
{
    public interface ICommand<TResult>
    {
        public ValidationResult GetValidationResult();
    }
}
