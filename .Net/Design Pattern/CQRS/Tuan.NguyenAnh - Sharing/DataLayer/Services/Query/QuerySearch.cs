using DataLayer.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Query
{
    public class QuerySearch : IQuerySearch
    {
        private string DatabaseConnectionString { get; set; }

        public QuerySearch(string databaseConnectionString)
        {
            DatabaseConnectionString = databaseConnectionString;
        }

        public AccountSummary SelectAccountSummary(string accountCode)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                return db.AccountSummaries.Single(x => x.AccountCode.Contains(accountCode));
            }
        }

        public UserSummary SelectUserSummary(string username)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                return db.UserSummaries.Single(x => x.LoginName.Contains(username) && x.UserRegistrationStatus == "Succeeded");
            }
        }

        public List<UserSummary> SelectListOfUserSummaries()
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                return db.UserSummaries.Where(x => x.UserRegistrationStatus == "Succeeded").ToList();
            }
        }

        public List<AccountSummary> SelectListOfAccountSummaries()
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                return db.AccountSummaries.ToList();
            }
        }

        public TransferSummary SelectTransferSummary(Guid transfer)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                return db.TransferSummaries.SingleOrDefault(x => x.TransferIdentifier == transfer);
            }
        }

        public bool UserExists(Func<UserSummary, bool> predicate)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                return db.UserSummaries.Any(predicate);
            }
        }
    }
}
