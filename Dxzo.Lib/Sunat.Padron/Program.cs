using System.ServiceProcess;

namespace Dxzo.Services
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SunatPadronService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
