using System.Data;

namespace Dxzo.Data.Common
{
    public abstract class DataAccessParameter
    {
        public object ParameterValue { get; set; }
        public ParameterDirection ParameterDirection { get; set; }
    }
    internal class DataAccessInternalParameter : DataAccessParameter
    {

    }   
}
