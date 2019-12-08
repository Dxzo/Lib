using System;
using System.Collections.Generic;
using System.Data.Common;
using log4net;

namespace Dxzo.Data.Utility
{
    public static class Helpers
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Helpers));

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
                _log.Error(ex.Message, ex);
                return null;
            }
        }
    }
}
