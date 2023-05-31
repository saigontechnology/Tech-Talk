using DataLayer.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Query
{
    public class QueryStore : IQueryStore
    {
        private string DatabaseConnectionString { get; set; }

        public QueryStore(string databaseConnectionString)
        {
            DatabaseConnectionString = databaseConnectionString;
        }

        #region Methods (Account)

        public void DecreaseAccountBalance(Guid accountId, decimal amount)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var account = db.AccountSummaries.Single(x => x.AccountIdentifier == accountId);
                account.AccountBalance -= amount;

                var person = db.UserSummaries.Single(x => x.UserIdentifier == account.OwnerIdentifier);
                person.TotalAccountBalance -= amount;

                db.SaveChanges();
            }
        }

        public void IncreaseAccountBalance(Guid accountId, decimal amount)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var account = db.AccountSummaries.Single(x => x.AccountIdentifier == accountId);
                account.AccountBalance += amount;

                var person = db.UserSummaries.Single(x => x.UserIdentifier == account.OwnerIdentifier);
                person.TotalAccountBalance += amount;

                db.SaveChanges();
            }
        }

        public void InsertAccount(Guid tenantId, Guid accountId, string accountCode, string accountStatus, Guid userId)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var account = new AccountSummary
                {
                    AccountIdentifier = accountId,
                    AccountCode = accountCode,
                    AccountStatus = accountStatus,
                    OwnerIdentifier = userId
                };
                db.AccountSummaries.Add(account);
                db.SaveChanges();

                if (accountStatus == "Open")
                {
                    var person = db.UserSummaries.Single(x => x.UserIdentifier == userId);
                    person.OpenAccountCount++;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateAccountStatus(Guid accountId, string accountStatus)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var account = db.AccountSummaries.Single(x => x.AccountIdentifier == accountId);
                account.AccountStatus = accountStatus;

                if (accountStatus != "Open")
                {
                    var person = db.UserSummaries.Single(x => x.UserIdentifier == account.OwnerIdentifier);
                    person.OpenAccountCount--;
                }

                db.SaveChanges();
            }
        }

        #endregion

        #region Methods (Person)

        public void DeleteUser(Guid userId)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var p = db.UserSummaries.Single(x => x.UserIdentifier == userId);
                db.UserSummaries.Remove(p);
                db.SaveChanges();
            }
        }

        public void InsertUser(Guid tenantId, Guid userId, string Name, DateTimeOffset UserRegistered
            , string loginname, string pass, string status)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var summary = new UserSummary
                {
                    UserIdentifier = userId,
                    Name = Name,
                    UserRegistered = UserRegistered,
                    LoginName = loginname,
                    LoginPassword = pass,
                    UserRegistrationStatus = status
                };
                db.UserSummaries.Add(summary);
                db.SaveChanges();
            }
        }

        public void UpdateName(Guid UserId, string name)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var person = db.UserSummaries.Single(x => x.UserIdentifier == UserId);
                person.Name = name;
                db.SaveChanges();
            }
        }

        #endregion

        #region Methods (Transfer)

        public void InsertTransfer(Guid tenant, Guid transfer, string status, Guid fromAccount, Guid toAccount, decimal amount)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var summary = new TransferSummary
                {
                    TransferIdentifier = transfer,
                    TransferStatus = status,
                    TransferAmount = amount,
                    FromAccountIdentifier = fromAccount,
                    ToAccountIdentifier = toAccount
                };
                db.TransferSummaries.Add(summary);
                db.SaveChanges();
            }
        }

        public void UpdateTransfer(Guid aggregateIdentifier, string status, string activity)
        {
            using (var db = new QueryDbContext(DatabaseConnectionString))
            {
                var summary = db.TransferSummaries.Single(x => x.TransferIdentifier == aggregateIdentifier);
                summary.TransferStatus = status;
                db.SaveChanges();
            }
        }

        #endregion
    }
}
