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
    public class UserQueryHandler
    {
		private readonly IQuerySearch _querySearch;

		public UserQueryHandler(IQueryQueue query, IQuerySearch querySearch)
		{
			_querySearch = querySearch;

			query.Subscribe<GetUserByUserNameQuery>(Handle);
		}

		public dynamic Handle(GetUserByUserNameQuery q)
		{
			return _querySearch.SelectUserSummary(q.UserName);
		}
	}
}
