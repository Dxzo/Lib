using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Dxzo.Utilities;

namespace Dxzo.Data.Client
{
    public sealed class SqlServerDataAccess : DataAccess
    {
        private readonly string _connectionString;
    
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _reader;

        private DataTable _data;
        private int _affected;

        private Log _log;

        public SqlServerDataAccess(string connectionStringName = "SqlServer")
        {
            try
            {
                _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
                _connection = new SqlConnection();

                _log = new Log(GetType());
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
        public override DataTable ExecuteQuery(string sentence)
        {
            try
            {
                _connection.ConnectionString = _connectionString;
                _connection.Open();

                _command = _connection.CreateCommand();
                _command.CommandText = sentence;
                _command.CommandType = CommandType.Text;

                _reader = _command.ExecuteReader();
                _data = new DataTable();
                _data.Load(_reader);

                return _data;
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
        public override DataTable ExecuteQuery(string storeProcedureName, IDictionary<string, object> parameters)
        {
            try
            {
                _connection.ConnectionString = _connectionString;
                _connection.Open();

                _command = _connection.CreateCommand();
                _command.CommandText = storeProcedureName;
                _command.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    _command.Parameters.AddWithValue(param.Key, param.Value);
                }

                _reader = _command.ExecuteReader();
                _data = new DataTable();
                _data.Load(_reader);

                return _data;
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
        public override int ExecuteCommand(string storeProcedureName, IDictionary<string, object> parameters)
        {
            try
            {
                _connection.ConnectionString = _connectionString;
                _connection.Open();

                _command = _connection.CreateCommand();
                _command.CommandText = storeProcedureName;
                _command.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    _command.Parameters.AddWithValue(param.Key, param.Value);
                }

                _affected = _command.ExecuteNonQuery();

                return _affected;
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
		///     Ejecucion de un procedimiento almacenado con parametros que devuelve la primera columna de la primera fila en un object, listo para ser casteado al tipo requerido.
		/// </summary>
        public override object ExecuteCommandScalar(string storeProcedureName, IDictionary<string, object> parameters)
        {
            try
            {
                _connection.ConnectionString = _connectionString;
                _connection.Open();

                _command = _connection.CreateCommand();
                _command.CommandText = storeProcedureName;
                _command.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    _command.Parameters.AddWithValue(param.Key, param.Value);
                }

                return _command.ExecuteScalar();
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
		///     Liberar los recursos utilizados de forma manual, aunque lo hace de forma implicita.
		/// </summary>
        public override void Dispose()
        {
            if (_reader != null)
            {
                if (!_reader.IsClosed) _reader.Close();
                _reader.Dispose();
            }

            if (_command != null) _command.Dispose();

            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open) _connection.Close();
                _connection.Dispose();
            }

            _reader = null;
            _command = null;
        }
    }
}
