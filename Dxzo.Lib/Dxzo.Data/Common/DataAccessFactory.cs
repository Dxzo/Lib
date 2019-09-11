using System;
using Dxzo.Data.Client;

namespace Dxzo.Data.Factory
{
    public class DataAccessFactory
    {
        public MySqlDataAccess CreateMySqlDataAccess() { throw new NotImplementedException(); }
        public SqlServerDataAccess CreateSqlServerDataAccess() { throw new NotImplementedException(); }
    }
}
