using System;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.ServiceProcess;
using System.Timers;
using Dxzo.Core.Utilities;

namespace Dxzo.Services
{
    public partial class SunatPadronService : ServiceBase
    {
        private Timer _timer;
        private Encoding _encoding;
        private Log _log;
        private string _culture;
        private string _day;
        private string _time;

        public SunatPadronService()
        {
            _encoding = Encoding.GetEncoding(ConfigurationManager.AppSettings["encoding"]);
            _culture = ConfigurationManager.AppSettings["culture"];
            _day = ConfigurationManager.AppSettings["day"];
            _time = ConfigurationManager.AppSettings["time"];

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(_culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(_culture);

            _log = new Log();

            _timer = new Timer();
            _timer.Elapsed += Process;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _log.Debug(string.Format("El servicio: {0}, esta iniciando...", ServiceName));
            _timer.Enabled = true;
        }
        protected override void OnStop()
        {
            _log.Debug(string.Format("El servicio: {0}, ha sido detenido.", ServiceName));
            Dispose(true);
        }
        protected void Process(object sender, ElapsedEventArgs ev)
        {
            _timer.Stop();

            try
            {
                if (Enum.TryParse(_day, out DayOfWeek day))
                {
                    if (DateTime.Now.DayOfWeek == day)
                    {
                        var time = TimeSpan.Parse(_time);
                        if (DateTime.Now.Hour == time.Hours && DateTime.Now.Minute == time.Minutes)
                        {
                            Core.Services.LecturaArchivo.Leer(_encoding);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.Debug(e.Message);
            }

            _timer.Start();
        }
    }
}
