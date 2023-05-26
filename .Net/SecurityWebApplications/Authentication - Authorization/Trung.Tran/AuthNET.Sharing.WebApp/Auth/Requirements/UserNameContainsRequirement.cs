using Microsoft.AspNetCore.Authorization;

namespace AuthNET.Sharing.WebApp.Auth.Requirements
{
    public class UserNameContainsRequirement : IAuthorizationRequirement
    {
        public UserNameContainsRequirement(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
