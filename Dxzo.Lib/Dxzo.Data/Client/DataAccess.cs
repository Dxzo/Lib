using System;
using System.Data;
using System.Collections.Generic;

namespace Dxzo.Data.Client
{
    public abstract class DataAccess : IDisposable
    {
        public ICollection<T> EjecutarConsulta<T>(string nombreSp, IDictionary<string, object> parametros) where T : class, new()
        {
            var datos = EjecutarConsulta(nombreSp, parametros);

            if (datos != null)
            {
                if (!(datos.Rows.Count > 0))
                    return null;
            } else
                return null;

            try
            {
                IList<T> listaTemp = new List<T>();

                foreach (DataRow reg in datos.Rows)
                {
                    T objeto = new T();
                    foreach (var propiedad in objeto.GetType().GetProperties())
                    {
                        try
                        {
                            var propiedadInfo = objeto.GetType().GetProperty(propiedad.Name);
                            propiedadInfo.SetValue(objeto, Convert.ChangeType(reg[propiedad.Name], propiedadInfo.PropertyType), null);
                        }
                        catch { continue; }
                    }
                    listaTemp.Add(objeto);
                }
                return listaTemp;
            }
            catch
            {
                return null;
            }
        }
        public abstract DataTable EjecutarConsulta(string consulta);
        public abstract DataTable EjecutarConsulta(string nombreSp, IDictionary<string, object> parametros);
        public abstract int EjecutarComando(string nombreSp, IDictionary<string, object> parametros);
        public abstract void Dispose();
    }
}
