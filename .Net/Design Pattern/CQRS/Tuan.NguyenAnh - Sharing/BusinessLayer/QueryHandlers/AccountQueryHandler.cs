using BusinessLayer.Queries;
using BusinessLayer.Services;
using DataLayer.Service.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.QueryHandlers
{
    public class AccountQueryHandler
    {
		private readonly IQuerySearch _querySearch;

		public AccountQueryHandler(IQueryQueue query, IQuerySearch querySearch)
		{
			_querySearch = querySearch;

			query.Subscribe<GetAccountByCodeQuery>(Handle);
		}

		public dynamic Handle(GetAccountByCodeQuery q)
		{
			return _querySearch.SelectAccountSummary(q.AccountCode);
		}

	}
}
