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
        public static DataAccessParameter Input(object value, DataAccessParameterSettings settings)
        {
            throw new NotImplementedException();
        }
        public static DataAccessParameter Output()
        {
            return new DataAccessInternalParameter
            {
                ParameterValue = DBNull.Value,
                ParameterDirection = ParameterDirection.Output
            };
        }
        public static DataAccessParameter InputOutput(object value)
        {
            return new DataAccessInternalParameter
            {
                ParameterValue = value,
                ParameterDirection = ParameterDirection.InputOutput
            };
        }
        public static DataAccessParameter InputOutput(object value, DataAccessParameterSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}