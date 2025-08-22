namespace PlanningBook.Domain.Interfaces
{
    public interface IQuery<TResult>
    {
        public ValidationResult GetValidationResult();
    }
}
