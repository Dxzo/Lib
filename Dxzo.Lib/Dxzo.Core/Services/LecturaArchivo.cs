using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dxzo.Data.Client;
using Dxzo.Core.Aplication;
using Dxzo.Core.Utilities;

namespace Dxzo.Core.Services
{
    public class LecturaArchivo
    {
        private static DataAccessSqlServer _data = new DataAccessSqlServer();
        private static Log _log = new Log();

        public static bool Leer(Encoding codificacion)
        {
            string archivo = null;
            string nombreArchivo = null;
            string desposito = null;
            string error = null;
            string procesado = null;
            string extension = null;
            bool resultado = false;

            var dic = Parametros.ObtenerParametros(Servicios.RucSunat);

            if (dic != null)
            {
                desposito = dic["RUTA_DEPOSITO"];
                error = dic["RUTA_ERROR"];
                procesado = dic["RUTA_PROCESADO"];
                extension = dic["EXTENSION_ARCHIVO"];
            }
            else
                return false;
            

            if (Directory.Exists(desposito))
            {
                try
                {
                    archivo = Directory.GetFiles(desposito, extension, SearchOption.TopDirectoryOnly).First();
                    nombreArchivo = Path.GetFileName(archivo);
                }
                catch { return false; }
            } else
                return false;

            try
            {
                string nombreSp = "SP_API_SUNAT_INSERTAR_CONTRIBUYENTE";

                using (var sr = new StreamReader(archivo, codificacion))
                {
                    string linea = null;

                    sr.ReadLine();
                    _log.Debug("- Comienza la lectura del archivo.");

                    EstablecerFase(Estados.EnProceso, nombreArchivo);

                    while ((linea = sr.ReadLine()) != null)
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

                        _data.EjecutarComando(nombreSp, parametros);
                    }
                    _log.Debug("- Termino la lectura del archivo.");

                    EstablecerFase(Estados.Procesado, nombreArchivo);
                }

                resultado = MoverArchivo(archivo, procesado, nombreArchivo);
                _log.Debug($"- Intentando mover archivo, resultado: {resultado}");

                return true;
            }
            catch (Exception e)
            {
                EstablecerFase(Estados.Error, nombreArchivo);

                resultado = MoverArchivo(archivo, error, nombreArchivo);
                _log.Debug($"- Intentando mover archivo, resultado: {resultado}");

                _log.Debug($"- {e.Message}");
                return false;
            }
        }
        public static bool MoverArchivo(string ubicacion, string destino, string nombreArchivo)
        { 
            try
            {
                var moverA = Path.Combine(destino, nombreArchivo);
                File.Move(ubicacion, moverA);

                return true;
            }
            catch (Exception e)
            {
                _log.Debug($"- {e.Message}");
                return false;
            }
        }
        public static void EstablecerFase(Estados estado, string nombreArchivo)
        {
            string nombreSp = "SP_API_SUNAT_CARGA_FASES";
            IDictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@ESTADO", estado},
                { "@NOMBRE_ARCHIVO", nombreArchivo }
            };

            _data.EjecutarComando(nombreSp, parametros);
        }
    }
}
