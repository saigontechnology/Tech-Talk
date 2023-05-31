using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Queries
{
    public class GetUserByUserNameQuery : IQuery
    {
        public string UserName { get; set; }

        public GetUserByUserNameQuery(string name)
        {
            UserName = name;
        }
    }
}
