using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dxzo.Data.Client;

namespace Dxzo.Core.Aplication
{
    public class Parametros
    {
        private static DataAccessSqlServer _data = new DataAccessSqlServer();

        public static IDictionary<string, string> ObtenerParametros(Servicios servicio)
        {
            string nombreSp = "SP_API_PARAMETROS_CONSULTA";
            IDictionary<string, object> parametros = new Dictionary<string, object>
            {
                {"@ID_SERVICIO", servicio}
            };

            var _dic = _data.EjecutarConsulta(nombreSp, parametros)
                            ?.AsEnumerable().ToDictionary(r => r["CLAVE"].ToString(), r => r["VALOR"].ToString());
            return _dic;
        }
    }
    public enum Servicios
    {
        RucSunat = 1
    }
}
