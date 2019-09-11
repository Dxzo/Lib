using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Dxzo.Data.Utilities
{
    public static class Helpers
    {
        public static IDictionary<string, object> GetParameterValuePairs(this DbParameterCollection parameterCollection)
        {
            try
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();
                
                foreach (DbParameter param in parameterCollection)
                    dic.Add(param.ParameterName, param.Value);
                
                return dic;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
