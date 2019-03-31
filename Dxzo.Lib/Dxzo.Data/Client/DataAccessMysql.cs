using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;

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

        public DataAccessMySql()
        {
            try
            {
                _cadenaConexion = ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
                _conexion = new MySqlConnection();
            }
            catch (Exception e)
            {
                //Log.Debug(e.Message, e.ToString());
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
                //Log.Debug(e.Message, e.ToString());
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
                //Log.Debug(e.Message, e.ToString());
                return null;
            }
            finally
            {
                Dispose();
            }
        }
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
                //Log.Debug(e.Message, e.ToString());
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

            _conexion = null;
            _lector = null;
            _comando = null;
            _cadenaConexion = null;
        }
    }
}
