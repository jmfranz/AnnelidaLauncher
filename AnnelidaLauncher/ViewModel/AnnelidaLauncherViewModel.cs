using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnnelidaLauncher.Model;


namespace AnnelidaLauncher.ViewModel
{
    class AnnelidaLauncherViewModel
    {
        public string MongoPath { get; private set; }
        public string DispPath { get; private set; }
        public string ThreeDPath { get; private set; }
        public string TwoDPath { get; private set; }

        public AnnelidaLauncherViewModel()
        {
            Launcher launcher = new Launcher();
            if (launcher.ConfigurationFile != null)
            {
                MongoPath = launcher.ConfigurationFile.MongoAddress;
                DispPath = launcher.ConfigurationFile.DispatcherAddress;
                ThreeDPath = launcher.ConfigurationFile.Annelida3DAddress;
                TwoDPath = launcher.ConfigurationFile.Annelida2DAddress;

            }
        }
    }
}
