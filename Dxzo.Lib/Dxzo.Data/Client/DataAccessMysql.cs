using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using Dxzo.Data.Utilities;

namespace Dxzo.Data.Client
{
    public sealed class DataAccessMySql : DataAccess
    {
        private string _cadenaConexion;

        private MySqlConnection _conexion;
        private MySqlCommand _comando;
        private MySqlDataReader _lector;

        private DataTable _datos;
        private int _afectadas;

        private Log _log;

        public DataAccessMySql(string nombreConexionString = "MySql")
        {
            try
            {
                _cadenaConexion = ConfigurationManager.ConnectionStrings[nombreConexionString].ConnectionString;
                _conexion = new MySqlConnection();

                _log = new Log();
            }
            catch (Exception e)
            {
                _log.Debug(e.Message);
                Dispose();
            }
        }
        /// <summary>
		///     Ejecucion de una consulta simple, sin parametros.
		/// </summary>
        public override DataTable EjecutarConsulta(string consulta)
        {
            try
            {
                _conexion.ConnectionString = _cadenaConexion;
                _conexion.Open();

                _comando = _conexion.CreateCommand();
                _comando.CommandText = consulta;
                _comando.CommandType = CommandType.Text;

                _lector = _comando.ExecuteReader();
                _datos = new DataTable();
                _datos.Load(_lector);

                return _datos;
            }
            catch (Exception e)
            {
                _log.Debug(e.Message);
                return null;
            }
            finally
            {
                Dispose();
            }
        }
        /// <summary>
		///     Ejecucion de un procedimiento almacenado con parametros que devuelve uno o más registros.
		/// </summary>
        public override DataTable EjecutarConsulta(string nombreSp, IDictionary<string, object> parametros)
        {
            try
            {
                _conexion.ConnectionString = _cadenaConexion;
                _conexion.Open();

                _comando = _conexion.CreateCommand();
                _comando.CommandText = nombreSp;
                _comando.CommandType = CommandType.StoredProcedure;

                foreach (var param in parametros)
                {
                    _comando.Parameters.AddWithValue(param.Key, param.Value);
                }

                _lector = _comando.ExecuteReader();
                _datos = new DataTable();
                _datos.Load(_lector);

                return _datos;
            }
            catch (Exception e)
            {
                _log.Debug(e.Message);
                return null;
            }
            finally
            {
                Dispose();
            }
        }
        /// <summary>
        ///     Ejecucion de un comando que devuelve el numero de registros afectados.
        /// </summary>
        public override int EjecutarComando(string nombreSp, IDictionary<string, object> parametros)
        {
            try
            {
                _conexion.ConnectionString = _cadenaConexion;
                _conexion.Open();

                _comando = _conexion.CreateCommand();
                _comando.CommandText = nombreSp;
                _comando.CommandType = CommandType.StoredProcedure;

                foreach (var param in parametros)
                {
                    _comando.Parameters.AddWithValue(param.Key, param.Value);
                }

                _afectadas = _comando.ExecuteNonQuery();

                return _afectadas;
            }
            catch (Exception e)
            {
                _log.Debug(e.Message);
                return 0;
            }
            finally
            {
                Dispose();
            }
        }
        /// <summary>
		///     Liberar los recursos utilizados.
		/// </summary>
        public override void Dispose()
        {
            if (_lector != null)
            {
                if (!_lector.IsClosed) _lector.Close();
                _lector.Dispose();
            }

            if (_comando != null) _comando.Dispose();

            if (_conexion != null)
            {
                if (_conexion.State == ConnectionState.Open) _conexion.Close();
                _conexion.Dispose();
            }

            _lector = null;
            _comando = null;
        }
    }
}
