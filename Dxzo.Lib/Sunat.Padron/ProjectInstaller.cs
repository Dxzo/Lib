using System.ComponentModel;
using System.Configuration.Install;

namespace Dxzo.Services
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
