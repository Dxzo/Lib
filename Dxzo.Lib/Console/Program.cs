using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dxzo.Data.Client;
using System.Configuration;
using System.Data.SqlClient;

namespace Consola
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string linea = string.Empty;
            int a = 0;
            try
            {
                DataAccessSqlServer data = new DataAccessSqlServer();
                string nombreSp = "SP_SUNAT_INSERTAR_CONTRIBUYENTE";

                using (var sr = new StreamReader(@"C:\Users\KENYI\Desktop\Sunat_api\test.txt", Encoding.UTF8))
                {
                    while((linea = sr.ReadLine()) != null)
                    {
                        var campos = linea.Split('|');

                        IDictionary<string, object> parametros = new Dictionary<string, object>
                        {
                            { "@RUC", campos[0] },
                            { "@NOMBRE_RAZON_SOCIAL", campos[1] },
                            { "@ESTADO_CONTRIBUYENTE", campos[2] },
                            { "@CONDICION_DOMICILIO", campos[3] },
                            { "@UBIGEO", campos[4] },
                            { "@TIPO_VIA", campos[5] },
                            { "@NOMBRE_VIA", campos[6] },
                            { "@CODIGO_ZONA", campos[7] },
                            { "@TIPO_ZONA", campos[8] },
                            { "@NUMERO", campos[9] },
                            { "@INTERIOR", campos[10] },
                            { "@LOTE", campos[11] },
                            { "@DEPARTAMENTO", campos[12] },
                            { "@MANZANA", campos[13] },
                            { "@KILOMETRO", campos[14] }
                        };

                        a = data.EjecutarComando(nombreSp, parametros);
                    }
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                
            }       
        }
    }
}
