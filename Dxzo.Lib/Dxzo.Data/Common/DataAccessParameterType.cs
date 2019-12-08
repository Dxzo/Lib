using System;
using System.Data;

namespace Dxzo.Data.Common 
{
    public abstract class DataAccessParameterType
    {
        public static DataAccessParameter Input(object value)
        {
            return new DataAccessInternalParameter
            {
                ParameterValue = value,
                ParameterDirection = ParameterDirection.Input
            };
        }
        public static DataAccessParameter Input(object value, DbType parameterDbType)
        {
            return new DataAccessInternalParameter
            {
                ParameterValue = value,
                ParameterDirection = ParameterDirection.Input,
                ParameterDbType = parameterDbType
            };
        }
        public static DataAccessParameter Output(DbType parameterDbType)
        {
            return new DataAccessInternalParameter
            {
                ParameterValue = DBNull.Value,
                ParameterDirection = ParameterDirection.Output,
                ParameterDbType = parameterDbType
            };
        }
        public static DataAccessParameter InputOutput(object value, DbType parameterDbType)
        {
            return new DataAccessInternalParameter
            {
                ParameterValue = value,
                ParameterDirection = ParameterDirection.InputOutput,
                ParameterDbType = parameterDbType
            };
        }
        public static DataAccessParameter Input(object value, DataAccessParameterSettings settings)
        {
            throw new NotImplementedException();
        }
        public static DataAccessParameter Output(DataAccessParameterSettings settings)
        {
            throw new NotImplementedException();
        }
        public static DataAccessParameter InputOutput(object value, DataAccessParameterSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}