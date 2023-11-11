using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSharing.Examples.Template
{
    public abstract class BenchmarkBase
    {
        protected SqlConnection _connection;
        public static string ConnectionString { get; } = Program.DBInfo.ConnectionString;
        //protected int i;

        protected void BaseSetup()
        {
            //i = 0;
            _connection = new SqlConnection(ConnectionString);
            _connection.Open();
        }

        //protected void Step()
        //{
        //    i++;
        //    if (i > 5000) i = 1;
        //}
    }
}
