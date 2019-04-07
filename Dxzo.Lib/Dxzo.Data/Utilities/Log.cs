using System;
using System.IO;
using System.Configuration;

namespace Dxzo.Data.Utilities
{
    public class Log
    {
        private const string _defaultpath = @"D:\log.txt";
        private static string _active;
        private static string _path;
        public Log()
        {
            _active = ConfigurationManager.AppSettings["log"];
            _path = ConfigurationManager.AppSettings["log_path"];
                
            if (_path == null)
                _path = _defaultpath;
            if (_active == null)
                _active = "false";
        }
        public bool Debug(string message)
        { 
            try
            {
                if (_active.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase))
                {
                    using (var sw = new StreamWriter(_path, true))
                    {
                        sw.WriteLine(
                            value: string.Format("{0}|{1}", DateTime.Now.ToString(), message)
                        );
                    }
                }
                return true;
            }
            catch { return false; }
        }
    }
}
