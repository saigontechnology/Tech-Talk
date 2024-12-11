namespace PlanningBook.Identity.Application.Helpers.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string passwordPlainText);
        bool Verify(string passwordPlainText, string passwordHash);
    }
}
