using Microsoft.AspNetCore.Identity;
using PlanningBook.Domain.Interfaces;

namespace PlanningBook.Identity.Infrastructure.Entities
{
    public class AccountToken : IdentityUserToken<Guid>, IDateAudited
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        // TODO: Need Cron Job to check and make IsRevoked Refresh token
        public DateTime RefreshTokenExpirationDate { get; set; }
        /// <summary>
        /// It use for force revoke: Both Token & Refresh Token is invalid now
        /// </summary>
        public bool IsRevoked { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
