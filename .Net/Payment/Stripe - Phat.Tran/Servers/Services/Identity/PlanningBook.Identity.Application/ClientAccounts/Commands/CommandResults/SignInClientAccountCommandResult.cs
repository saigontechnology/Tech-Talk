namespace PlanningBook.Identity.Application.ClientAccounts.Commands.CommandResults
{
    public class SignInClientAccountCommandResult
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
