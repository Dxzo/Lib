using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dxzo.Data.Client;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Globalization;
using System.Data;
using Dxzo.Data.Utilities;
using Dxzo.Core.Services;

namespace Consola
{
    class Program
    {
        static void Main(string[] args)
        {

            Encoding _encoding = Encoding.GetEncoding(ConfigurationManager.AppSettings["encoding"]);
            string _culture = ConfigurationManager.AppSettings["culture"];
            string _day = ConfigurationManager.AppSettings["day"];
            string _time = ConfigurationManager.AppSettings["time"];

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(_culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(_culture);

            try
            {
                if (Enum.TryParse(_day, out DayOfWeek day))
                {
                    if (DateTime.Now.DayOfWeek == day)
                    {
                        var time = TimeSpan.Parse(_time);
                        if (DateTime.Now.Hour == time.Hours && DateTime.Now.Minute == time.Minutes)
                        {
                            LecturaArchivo.Leer(_encoding);
                        }
                    }
                }
                
                Console.WriteLine("Finalizo ");
                Console.ReadLine();
            }
            catch (Exception e)
            {
            }       
        }
    }
}
