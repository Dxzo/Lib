using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dxzo.Data.Utilities;
using Dxzo.Data.Common;
using Dxzo.Utilities;

namespace Dxzo.Data.Client
{
    public sealed class SqlServerDataAccess : DataAccess
    {
        private readonly string _connectionString;
    
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _reader;
        private SqlTransaction _transaction;

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

        #region Properties
        public bool Transaction { private get; set; }
        public IDictionary<string, object> CommandParameters { get; private set; }
        #endregion

        #region Sync methods
        /// <summary>
        /// Confirma todos los cambios realizados solo si la propiedad Transaction esta en true, de lo contrario se lanza una excepcion.
        /// </summary>
        public override void Commit()
        {
            if (_transaction != null)
            {
                if (_transaction.Connection.State == ConnectionState.Open)
                {
                    _transaction.Commit();

                    Transaction = false;
                    Dispose();
                }
            }
            else
            {
                throw new InvalidOperationException("No puede realizar un commit si las transacciones no estan activas.");
            }
        }
        /// <summary>
        /// Revierte todos los cambios realizados solo si la propiedad Transaction esta en true, de lo contrario se lanza una excepcion.
        /// </summary>
        public override void Rollback()
        {
            if (_transaction != null)
            {
                if (_transaction.Connection.State == ConnectionState.Open)
                {
                    _transaction.Rollback();

                    Transaction = false;
                    Dispose();
                }
            }
            else
            {
                throw new InvalidOperationException("No puede realizar un rollback si las transacciones no estan activas.");
            }
        }
        /// <summary>
        /// Ejecucion de una consulta simple, sin parametros.
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
                CommandParameters = _command.Parameters.GetParameterValuePairs();
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
		/// Ejecucion de un procedimiento almacenado con parametros que devuelve uno o más registros.
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
                    var parameter = _command.Parameters.AddWithValue(param.Key, param.Value);

                    try
                    {
                        var parameterObject = (DataAccessParameter)param.Value;

                        parameter.Value = parameterObject.ParameterValue;
                        parameter.Direction = parameterObject.ParameterDirection;
                    }
                    catch { parameter.Direction = ParameterDirection.Input; }
                }

                _reader = _command.ExecuteReader();
                CommandParameters = _command.Parameters.GetParameterValuePairs();
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
        /// Ejecucion de un comando que devuelve el numero de registros afectados.
        /// </summary>
        public override int ExecuteCommand(string storeProcedureName, IDictionary<string, object> parameters)
        {
            try
            {
                Settings(storeProcedureName, parameters);

                _affected = _command.ExecuteNonQuery();
                CommandParameters = _command.Parameters.GetParameterValuePairs();

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
        /// Ejecucion de un procedimiento almacenado con parametros que devuelve la primera columna de la primera fila en un object, listo para ser casteado al tipo requerido.
        /// </summary>
        public override object ExecuteCommandScalar(string storeProcedureName, IDictionary<string, object> parameters)
        {
            try
            {
                Settings(storeProcedureName, parameters);

                var result = _command.ExecuteScalar();
                CommandParameters = _command.Parameters.GetParameterValuePairs();

                return result;
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
        #endregion

        #region Async methods
        public override async Task<DataTable> ExecuteQueryAsync(string sentence)
        {
            try
            {
                _connection.ConnectionString = _connectionString;
                await _connection.OpenAsync();

                _command = _connection.CreateCommand();
                _command.CommandText = sentence;
                _command.CommandType = CommandType.Text;

                var dataReaderTask = _command.ExecuteReaderAsync();
                CommandParameters = _command.Parameters.GetParameterValuePairs();
                _data = new DataTable();
                _data.Load((SqlDataReader)await dataReaderTask);

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
        public override async Task<DataTable> ExecuteQueryAsync(string storeProcedureName, IDictionary<string, object> parameters)
        {
            try
            {
                _connection.ConnectionString = _connectionString;
                await _connection.OpenAsync();

                _command = _connection.CreateCommand();
                _command.CommandText = storeProcedureName;
                _command.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    var parameter = _command.Parameters.AddWithValue(param.Key, param.Value);

                    try
                    {
                        var parameterObject = (DataAccessParameter)param.Value;

                        parameter.Value = parameterObject.ParameterValue;
                        parameter.Direction = parameterObject.ParameterDirection;
                    }
                    catch { parameter.Direction = ParameterDirection.Input; }
                }

                var dataReaderTask = _command.ExecuteReaderAsync();
                CommandParameters = _command.Parameters.GetParameterValuePairs();
                _data = new DataTable();
                _data.Load((SqlDataReader)await dataReaderTask);

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
        public override async Task<int> ExecuteCommandAsync(string storeProcedureName, IDictionary<string, object> parameters)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.ConnectionString = _connectionString;
                    await _connection.OpenAsync();
                }

                _command = _connection.CreateCommand();

                if (Transaction)
                {
                    if (_transaction == null)
                    {
                        _transaction = _connection.BeginTransaction();
                    }
                    _command.Transaction = _transaction;
                }

                _command.CommandText = storeProcedureName;
                _command.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    var parameter = _command.Parameters.AddWithValue(param.Key, param.Value);

                    try
                    {
                        var parameterObject = (DataAccessParameter)param.Value;

                        parameter.Value = parameterObject.ParameterValue;
                        parameter.Direction = parameterObject.ParameterDirection;
                    }
                    catch { parameter.Direction = ParameterDirection.Input; }
                }

                _affected = await _command.ExecuteNonQueryAsync();
                CommandParameters = _command.Parameters.GetParameterValuePairs();

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
        public override async Task<object> ExecuteCommandScalarAsync(string storeProcedureName, IDictionary<string, object> parameters)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.ConnectionString = _connectionString;
                    await _connection.OpenAsync();
                }

                _command = _connection.CreateCommand();

                if (Transaction)
                {
                    if (_transaction == null)
                    {
                        _transaction = _connection.BeginTransaction();
                    }
                    _command.Transaction = _transaction;
                }

                _command.CommandText = storeProcedureName;
                _command.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    var parameter = _command.Parameters.AddWithValue(param.Key, param.Value);

                    try
                    {
                        var parameterObject = (DataAccessParameter)param.Value;

                        parameter.Value = parameterObject.ParameterValue;
                        parameter.Direction = parameterObject.ParameterDirection;
                    }
                    catch { parameter.Direction = ParameterDirection.Input; }
                }

                var result = await _command.ExecuteScalarAsync();
                CommandParameters = _command.Parameters.GetParameterValuePairs();

                return result;
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
        #endregion
        
        #region Private methods
        private void Settings(string storeProcedureName, IDictionary<string, object> parameters) {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.ConnectionString = _connectionString;
                    _connection.Open();
                }

                _command = _connection.CreateCommand();

                if (Transaction)
                {
                    if (_transaction == null)
                    {
                        _transaction = _connection.BeginTransaction();
                    }
                    _command.Transaction = _transaction;
                }

                _command.CommandText = storeProcedureName;
                _command.CommandType = CommandType.StoredProcedure;

                foreach (var param in parameters)
                {
                    var parameter = _command.Parameters.AddWithValue(param.Key, param.Value);

                    try
                    {
                        var parameterObject = (DataAccessParameter)param.Value;

                        parameter.Value = parameterObject.ParameterValue;
                        parameter.Direction = parameterObject.ParameterDirection;
                    }
                    catch { parameter.Direction = ParameterDirection.Input; }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Resources
        /// <summary>
        /// Liberar los recursos utilizados de forma manual y cierra la conexion a excepcion de que la propiedad Transaction este en true, 
        /// en ese caso la conexion estara activa hasta que se ejecute el commit o rollback respectivo.
        /// </summary>
        public override void Dispose()
        {
            if (_reader != null)
            {
                if (!_reader.IsClosed) _reader.Close();
                _reader.Dispose();
            }

            if (_command != null) _command.Dispose();

            if (!Transaction)
            {
                if (_connection != null)
                {
                    if (_connection.State == ConnectionState.Open) _connection.Close();
                    _connection.Dispose();

                    _transaction = null;
                }
            }

            if (_data != null) _data = null;

            _reader = null;
            _command = null;
        }
        #endregion
    }
}
