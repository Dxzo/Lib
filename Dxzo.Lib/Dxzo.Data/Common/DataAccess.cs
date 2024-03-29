﻿using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dxzo.Data.Factory;
using log4net;

namespace Dxzo.Data.Common
{
    public abstract class DataAccess : IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DataAccess));

        public static DataAccessFactory Factory { get; }

        public abstract void Commit();
        public abstract void Rollback();
        
        public ICollection<T> ExecuteQuery<T>(string storeProcedureName, IDictionary<string, object> parameters) where T : class, new()
        {
            var data = ExecuteQuery(storeProcedureName, parameters);

            if (data != null)
            {
                if (!(data.Rows.Count > 0))
                    return null;
            } else
                return null;

            try
            {
                IList<T> tempList = new List<T>();

                foreach (DataRow reg in data.Rows)
                {
                    T _object = new T();
                    foreach (var property in _object.GetType().GetProperties())
                    {
                        try
                        {
                            var propertyInfo = _object.GetType().GetProperty(property.Name);
                            propertyInfo.SetValue(_object, Convert.ChangeType(reg[property.Name], propertyInfo.PropertyType), null);
                        }
                        catch (Exception ex) { _log.Error(ex.Message, ex); continue; }
                    }
                    tempList.Add(_object);
                }
                return tempList;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                return null;
            }
        }
        public Task<ICollection<T>> ExecuteQueryAsync<T>(string storeProcedureName, IDictionary<string, object> parameters) where T : class, new()
        {
            throw new NotImplementedException();
        }
        public abstract DataTable ExecuteQuery(string sentence);
        public abstract Task<DataTable> ExecuteQueryAsync(string sentence);
        public abstract DataTable ExecuteQuery(string storeProcedureName, IDictionary<string, object> parameters);
        public abstract Task<DataTable> ExecuteQueryAsync(string storeProcedureName, IDictionary<string, object> parameters);
        public abstract int ExecuteCommand(string storeProcedureName, IDictionary<string, object> parameters);
        public abstract Task<int> ExecuteCommandAsync(string storeProcedureName, IDictionary<string, object> parameters);
        public abstract object ExecuteCommandScalar(string storeProcedureName, IDictionary<string, object> parameters);
        public abstract Task<object> ExecuteCommandScalarAsync(string storeProcedureName, IDictionary<string, object> parameters);
        public abstract void Dispose();
    }
}
