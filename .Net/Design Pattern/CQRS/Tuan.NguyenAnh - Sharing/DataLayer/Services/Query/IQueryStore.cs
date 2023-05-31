using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Query
{
    public interface IQueryStore
    {
        void DeleteUser(Guid userId);
        void InsertUser(Guid tenantId, Guid userId, string Name, DateTimeOffset UserRegistered, string loginname, string pass, string status);
        void UpdateName(Guid userId, string Name);

        void DecreaseAccountBalance(Guid account, decimal amount);
        void IncreaseAccountBalance(Guid account, decimal amount);
        void InsertAccount(Guid tenantId, Guid accountId, string accountCode, string accountStatus, Guid userId);
        void UpdateAccountStatus(Guid account, string status);

        void InsertTransfer(Guid tenant, Guid transfer, string status, Guid fromAccount, Guid toAccount, decimal amount);
        void UpdateTransfer(Guid aggregateIdentifier, string status, string activity);
    }
}
