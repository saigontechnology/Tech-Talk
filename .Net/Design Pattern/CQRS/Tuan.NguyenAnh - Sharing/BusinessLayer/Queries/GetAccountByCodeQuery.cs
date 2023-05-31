using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Queries
{
    public class GetAccountByCodeQuery : IQuery
    {
        public string AccountCode { get; set; }

        public GetAccountByCodeQuery(string code)
        {
            AccountCode = code;
        }
    }
}
