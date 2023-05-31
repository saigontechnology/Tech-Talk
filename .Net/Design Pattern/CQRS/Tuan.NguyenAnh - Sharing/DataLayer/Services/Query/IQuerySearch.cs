using DataLayer.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Query
{
    public interface IQuerySearch
    {
        AccountSummary SelectAccountSummary(string accountCode);
        UserSummary SelectUserSummary(string userName);
        TransferSummary SelectTransferSummary(Guid transaction);
        List<UserSummary> SelectListOfUserSummaries();
        List<AccountSummary> SelectListOfAccountSummaries();
        bool UserExists(Func<UserSummary, bool> predicate);
    }
}
