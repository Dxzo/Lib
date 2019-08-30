using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dxzo.Data.Client;

namespace Dxzo.Data.Factory
{
    public class DataAccessFactory
    {
        public MySqlDataAccess CreateMySqlDataAccess() { throw new NotImplementedException(); }
        public SqlServerDataAccess CreateSqlServerDataAccess() { throw new NotImplementedException(); }
    }
}
