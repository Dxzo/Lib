using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
