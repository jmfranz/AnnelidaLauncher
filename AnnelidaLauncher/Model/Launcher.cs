using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace AnnelidaLauncher.Model
{
    class Launcher
    {
        private readonly HashSet<MonitoredProcess> annelidaProcesses;
        private StatsPerformanceMonitor statsPerformanceMonitor;
        private WatchDogListener watchDogListener;

        public ConfigurationFile ConfigurationFile { get; private set; }
        public Launcher()
        {
            annelidaProcesses = new HashSet<MonitoredProcess>();

            if (LoadConfigFile())
            {
               
                LaunchApplications(ConfigurationFile);
                statsPerformanceMonitor = new StatsPerformanceMonitor(annelidaProcesses);
                watchDogListener = new WatchDogListener(9998, annelidaProcesses);

            }
        }

        private bool LoadConfigFile()
        {
            try
            {
                using (StreamReader file = File.OpenText(@"config.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    ConfigurationFile = (ConfigurationFile)serializer.Deserialize(file, typeof(ConfigurationFile));
                }

                return true;
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Arquivo de configuração (config.json) não encontrado.","Erro", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
        }

        private void LaunchApplications(ConfigurationFile config)
        {
            MonitoredProcess mongo = new MonitoredProcess("MongoDB", config.MongoAddress,false);
            annelidaProcesses.Add(mongo);
            MonitoredProcess dispactcher = new MonitoredProcess("Dispatcher", config.DispatcherAddress,true);
            annelidaProcesses.Add(dispactcher);
            return;
            MonitoredProcess annelida3D = new MonitoredProcess("Annelida 3D", config.Annelida3DAddress,false);
            MonitoredProcess annelida2D = new MonitoredProcess("Annelida 2D", config.Annelida2DAddress,false);
            return;
        }

    }
}
