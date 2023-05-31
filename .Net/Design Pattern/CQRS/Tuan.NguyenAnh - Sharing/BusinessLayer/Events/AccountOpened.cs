
namespace BusinessLayer.Events
{
    public class AccountOpened : EventBase
    {
        public Guid Owner { get; set; }
        public string Code { get; set; }

        public AccountOpened(Guid owner, string code)
        {
            Owner = owner;
            Code = code;
        }
    }
}
